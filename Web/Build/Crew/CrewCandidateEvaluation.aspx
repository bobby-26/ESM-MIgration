<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCandidateEvaluation.aspx.cs"
    Inherits="CrewCandidateEvaluation" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="OffSigner" Src="~/UserControls/UserControlCrewOnboard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlZone" Src="~/UserControls/UserControlZone.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPool.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Assessment</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%=confirm.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCandidateEvaluation" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:TabStrip ID="CrewMenuGeneral" runat="server" OnTabStripCommand="CrewMenuGeneral_TabStripCommand"></eluc:TabStrip>
            <eluc:TabStrip ID="CrewUnLock" runat="server" Visible="false"></eluc:TabStrip>
            <eluc:TabStrip ID="CopyProposal" runat="server" OnTabStripCommand="CopyProposal_TabStripCommand"></eluc:TabStrip>
            <asp:Button ID="confirm" runat="server" Text="confirm" OnClick="btnApprove_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <table id="table1" runat="server" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td colspan="2">
                        <a href="3869_DE 29 -FOLLOW_UP_SHEET.doc" target="_blank">Offer Letter</a>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <telerik:RadLabel ID="lblnote" runat="server" Text="Note:THIS PROPOSAL IS LOCKED AND HENCE FOR PD -SEE THE PROPSAL ATTACHMENT-PDFORM"
                            ForeColor="Red">
                        </telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">

                        <b>
                            <telerik:RadLabel ID="RadLabel1" runat="server" Text="Documents Check(Mandatory only for Joiners new to company)"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="RadLabel2" runat="server" Text="Does the seafarer hold valid flag documents for the vessel joining"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadRadioButtonList ID="chkFlagDoc" runat="server" Layout="Flow" Columns="3" Direction="Horizontal"
                            AutoPostBack="true" OnSelectedIndexChanged="OnSelect">
                            <Items>
                                <telerik:ButtonListItem Value="1" Text="Yes" />
                                <telerik:ButtonListItem Value="2" Text="Not Applicable" />
                                <telerik:ButtonListItem Value="0" Text="Applied" />
                            </Items>
                        </telerik:RadRadioButtonList>

                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel3" runat="server" Text="Checked By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCheckFlagDoc" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <b>
                            <telerik:RadLabel ID="RadLabel4" runat="server" Text="Check for renewal of following Documents(Mandatory only for ex-hands)"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="RadLabel5" runat="server" Text="Has the seafarer renewed his Passport after his last sign off"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadRadioButtonList ID="chkPassport" runat="server" Layout="Flow" Columns="3" Direction="Horizontal"
                            AutoPostBack="true" OnSelectedIndexChanged="OnSelect">
                            <Items>
                                <telerik:ButtonListItem Value="1" Text="Yes" />
                                <telerik:ButtonListItem Value="2" Text="Not Applicable" />
                                <telerik:ButtonListItem Value="0" Text="Applied" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel6" runat="server" Text="Checked By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCheckPassport" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="RadLabel7" runat="server" Text="Has the seafarer renewed his COC after his last sign off"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadRadioButtonList ID="chkCOC" runat="server" Layout="Flow" Columns="3" Direction="Horizontal"
                            AutoPostBack="true" OnSelectedIndexChanged="OnSelect">
                            <Items>
                                <telerik:ButtonListItem Value="1" Text="Yes" />
                                <telerik:ButtonListItem Value="2" Text="Not Applicable" />
                                <telerik:ButtonListItem Value="0" Text="Applied" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel8" runat="server" Text="Checked By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCheckCOC" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="RadLabel9" runat="server" Text="Has the seafarer renewed his GMDSS Goc after his last sign off"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadRadioButtonList ID="chkGOC" runat="server" Layout="Flow" Columns="3" Direction="Horizontal"
                            AutoPostBack="true" OnSelectedIndexChanged="OnSelect">
                            <Items>
                                <telerik:ButtonListItem Value="1" Text="Yes" />
                                <telerik:ButtonListItem Value="2" Text="Not Applicable" />
                                <telerik:ButtonListItem Value="0" Text="Applied" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel10" runat="server" Text="Checked By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCheckGOC" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="RadLabel11" runat="server" Text="Has the seafarer renewed his US Visa after his last contract"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadRadioButtonList ID="chkUSVisa" runat="server" Layout="Flow" Columns="3" Direction="Horizontal"
                            AutoPostBack="true" OnSelectedIndexChanged="OnSelect">
                            <Items>
                                <telerik:ButtonListItem Value="1" Text="Yes" />
                                <telerik:ButtonListItem Value="2" Text="Not Applicable" />
                                <telerik:ButtonListItem Value="0" Text="Applied" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel12" runat="server" Text="Checked By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCheckUSVisa" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="RadLabel13" runat="server" Text="Has the seafarer renewed his CDC after his last contract"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadRadioButtonList ID="chkCDCRenewed" runat="server" Layout="Flow" Columns="3" Direction="Horizontal"
                            AutoPostBack="true" OnSelectedIndexChanged="OnSelect">
                            <Items>
                                <telerik:ButtonListItem Value="1" Text="Yes" />
                                <telerik:ButtonListItem Value="2" Text="Not Applicable" />
                                <telerik:ButtonListItem Value="0" Text="Applied" />
                            </Items>
                        </telerik:RadRadioButtonList>

                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel14" runat="server" Text="Checked By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCDCRenewedCB" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="RadLabel15" runat="server" Text="Have the original Licenses been sighted"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadCheckBox ID="chkOrgLicSig" runat="server" AutoPostBack="true" OnCheckedChanged="OnClick"></telerik:RadCheckBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel16" runat="server" Text="Checked By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtOrgLicSigCB" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="RadLabel17" runat="server" Text="Have the Mandatory STCW and Training Certificates Sighted"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadCheckBox ID="chkCertSig" runat="server" AutoPostBack="true" OnCheckedChanged="OnClick"></telerik:RadCheckBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel18" runat="server" Text="Checked By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCertSigCB" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="RadLabel19" runat="server" Text="Has the experience confirmed by interview"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadCheckBox ID="chkExpConf" runat="server" AutoPostBack="true" OnCheckedChanged="OnClick"></telerik:RadCheckBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel20" runat="server" Text="Checked By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtExpConfCB" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="RadLabel21" runat="server" Text="Experience details verified"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadCheckBox ID="chkOtherDet" runat="server" AutoPostBack="true" OnCheckedChanged="OnClick"></telerik:RadCheckBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel22" runat="server" Text="Checked By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtOtherDetCB" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="RadLabel24" runat="server" Text="Are the address and contact details latest"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadCheckBox ID="chkAddressLatest" runat="server" AutoPostBack="true" OnCheckedChanged="OnClick"></telerik:RadCheckBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel23" runat="server" Text="Checked By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAddressLatest" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <b>
                            <telerik:RadLabel ID="RadLabel61" runat="server" Text="Recommended Courses"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
            </table>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewRecommendedCourses" runat="server" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="false" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvCrewRecommendedCourses_NeedDataSource" EnableHeaderContextMenu="true" ShowFooter="false" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Course" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCourse" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Recommended By" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRecommendedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECOMMENDEDBYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Recommended Date" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRecommendedDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDRECOMMENDEDDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="false" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br />
            <b>
                <telerik:RadLabel ID="RadLabel60" runat="server" Text="Last Vessel"></telerik:RadLabel>
            </b>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvLastVessel" runat="server" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="false" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvLastVessel_NeedDataSource" EnableHeaderContextMenu="true" ShowFooter="false" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign On Date" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignOnDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDSIGNONDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign Off Date" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignOffDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDSIGNOFFDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Duration" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDuration" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDURATION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reason" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReason" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREASON") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNOFFREMARKS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="false" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br />
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReasonPreCom" runat="server" Text="Reason for change from previous company"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtChnageReason" runat="server" TextMode="MultiLine" Width="95%"
                            Height="30px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel26" runat="server" Text="Any major incidents / accidents encountered"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtIncedents" runat="server" TextMode="MultiLine" Width="95%"
                            Height="30px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel27" runat="server" Text="Dock Attended"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtDockAttended" runat="server" TextMode="MultiLine" Width="95%"
                            Height="30px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel28" runat="server" Text="Vetting Done"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtVettingDone" runat="server" TextMode="MultiLine" Width="95%"
                            Height="30px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel29" runat="server" Text="Cargo Carried"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtCargoCarried" runat="server" TextMode="MultiLine" Width="95%"
                            Height="30px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel30" runat="server" Text="Back Check Remarks"></telerik:RadLabel>
                        <span id="Span1" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                        <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip1" Width="300px" ShowEvent="onmouseover"
                            RelativeTo="Element" Animation="Fade" TargetControlID="Span1" IsClientID="true"
                            HideEvent="ManualClose" Position="MiddleRight" EnableRoundedCorners="true"
                            Text="To Enable Back Check Remarks TextBox, Please Select Back Checked">
                        </telerik:RadToolTip>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtBackChk" runat="server" TextMode="MultiLine" Width="95%" ReadOnly="true"
                            Height="30px" CssClass="readonlytextbox">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel31" runat="server" Text="Date of Availability"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtDOA" runat="server" Width="40%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel34" runat="server" Text="Back Checked"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadCheckBox ID="chkBackedChecked" runat="server" AutoPostBack="true" OnCheckedChanged="OnBackCheckedClick"></telerik:RadCheckBox>
                    </td>

                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="RadLabel33" runat="server" Text="Compiled By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCompliedBy" runat="server" Width="40%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel32" runat="server" Text="Salary Agreed (USD)"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <eluc:Number ID="txtSalAgreed" runat="server" CssClass="input_mandatory" MaxLength="12" Width="40%"
                            DecimalPlace="2" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel35" runat="server" Text="Expected Joining Vessel"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <eluc:Vessel ID="ddlVessel" runat="server" VesselsOnly="true" AppendDataBoundItems="true" Width="40%"
                            CssClass="input_mandatory" AutoPostBack="true" OnTextChangedEvent="BindOffSigner" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel36" runat="server" Text="Posted Rank"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox runat="server" ID="txtPostedRank" CssClass="readonlytextbox" ReadOnly="true" Width="40%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel37" runat="server" Text="Expected Joining Date"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <eluc:Date ID="txtJoinDate" runat="server" CssClass="input_mandatory" />
                        <span id="Span2" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                        <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip2" Width="300px" ShowEvent="onmouseover"
                            RelativeTo="Element" Animation="Fade" TargetControlID="Span2" IsClientID="true"
                            HideEvent="ManualClose" Position="MiddleRight" EnableRoundedCorners="true"
                            Text="Note: This date is used for checking validity of travel documents
                                    and Licenses while proposing">
                        </telerik:RadToolTip>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel39" runat="server" Text="Documents in Hand"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadCheckBox ID="chkDocsinHand" runat="server"></telerik:RadCheckBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel40" runat="server" Text="Rank of the Person to be Relieved"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <eluc:Rank ID="ddlRank" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                            AutoPostBack="true" OnTextChangedEvent="ddlRank_TextChanged" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel41" runat="server" Text="Off-Signer Name"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <eluc:OffSigner ID="ddlOffSigner" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" />
                        <telerik:RadTextBox ID="txtOffsigner" runat="server" CssClass="readonlytextbox" Width="200px" ReadOnly="true"></telerik:RadTextBox>
                        <telerik:RadCheckBox ID="chkNoRelevee" runat="server" AutoPostBack="true" Text="No Relievee"
                            OnCheckedChanged="OnNoRelieveeClick">
                        </telerik:RadCheckBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel42" runat="server" Text="Does the seafarer has previous rating experience?"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <eluc:Hard ID="ucRatingExp" AppendDataBoundItems="true" CssClass="input_mandatory"
                            runat="server" HardTypeCode="43" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel43" runat="server" Text="Zone(joiner)"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <eluc:UserControlZone ID="ddlZone" runat="server" AppendDataBoundItems="true"
                            Enabled="false" />
                        <telerik:RadLabel ID="RadLabel44" runat="server" Text="Pool(joiner)"></telerik:RadLabel>
                        <eluc:Pool ID="ddlPool" runat="server" AppendDataBoundItems="true" CssClass="readonlytextbox"
                            Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel45" runat="server" Text="Both the Seniors Officers (Deck/Engine) Joining together?"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadCheckBox ID="chkJoinTogether" runat="server" AutoPostBack="true"
                            OnCheckedChanged="onChkJoinTogetherClick">
                        </telerik:RadCheckBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel46" runat="server" Text="Joining Officer's Rank"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtJoinOfficerRankid" runat="server" Visible="false"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtJoinOfficerRank" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                            Enabled="false">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel47" runat="server" Text="Experience(in months)"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <eluc:Number ID="txtExperience" runat="server" CssClass="readonlytextbox" MaxLength="50" Style="text-align: right;" Enabled="false"
                            DecimalPlace="2" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel48" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtRemarksOfficer" runat="server" TextMode="MultiLine" Width="300px"
                            Height="30px" CssClass="readonlytextbox" Enabled="false">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr runat="server" visible="false">
                    <td>
                        <telerik:RadLabel ID="RadLabel49" runat="server" Text="Short Contract"></telerik:RadLabel>
                    </td>
                    <td colspan="5">
                        <telerik:RadCheckBox ID="chkProposing" runat="server"></telerik:RadCheckBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <telerik:RadLabel ID="RadLabel50" runat="server" Text="General Remarks"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <telerik:RadEditor ID="txtGeneralRemarks" Width="99%" runat="server" EmptyMessage="" ImageManager-EnableImageEditor="false" RenderMode="Lightweight" SkinID="DefaultSetOfTools">
                            <Modules>
                                <telerik:EditorModule Name="RadEditorHtmlInspector" Enabled="false" Visible="false" />
                                <telerik:EditorModule Name="RadEditorNodeInspector" Enabled="false" Visible="false" />
                                <telerik:EditorModule Name="RadEditorDomInspector" Enabled="false" />
                                <telerik:EditorModule Name="RadEditorStatistics" Enabled="false" />
                            </Modules>
                        </telerik:RadEditor>
                    </td>
                </tr>
            </table>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvInterviewBy" runat="server" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvInterviewBy_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvInterviewBy_ItemDataBound"
                OnItemCommand="gvInterviewBy_ItemCommand" OnUpdateCommand="gvInterviewBy_UpdateCommand" ShowFooter="True" OnDeleteCommand="gvInterviewBy_DeleteCommand"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Interviewed By" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInterviewedById" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINTERVIEWBYID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblInterviewBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINTERVIEWBY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblInterviewedByIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINTERVIEWBYID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtInterviewedByEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINTERVIEWBY") %>'
                                    CssClass="gridinput_mandatory" MaxLength="100">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtInterviewedByAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="100">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Interview Date" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInterviewDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDINTERVIEWDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtInterviewDateEdit" CssClass="input_mandatory"
                                    Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDINTERVIEWDATE")) %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date runat="server" ID="txtInterviewDateAdd" CssClass="input_mandatory" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtRemarksEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtRemarksAdd" runat="server"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit" ToolTip="Edit" CommandName="EDIT" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdDelete" CommandName="DELETE" ToolTip="Delete" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ID="cmdSave" CommandName="Update" ToolTip="Save" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ID="cmdCancel" CommandName="Cancel" ToolTip="Cancel" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ID="cmdAdd" CommandName="Add" ToolTip="Add New" Width="20px" Height="20px">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td colspan="6">
                        <asp:DataList ID="dlstEvaluation" runat="server" RepeatColumns="3" RepeatLayout="Table">
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="Label1" runat="server" Font-Bold="true" Text='<%#DataBinder.Eval(Container,"DataItem.FLDHARDNAME")%>'></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadRadioButtonList ID="rblEvaluation" runat="server" Layout="Flow" Direction="Horizontal"
                                                AutoPostBack="true">
                                                <Items>
                                                    <telerik:ButtonListItem Value="1" Text="Very Good" />
                                                    <telerik:ButtonListItem Value="2" Text="Good" />
                                                    <telerik:ButtonListItem Value="3" Text="Fair" />
                                                    <telerik:ButtonListItem Value="4" Text="Poor" />
                                                </Items>
                                            </telerik:RadRadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:DataList>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
            </table>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td colspan="6">
                        <b>
                            <telerik:RadLabel ID="RadLabel51" runat="server" Text="Hand over remarks to operation team"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel52" runat="server" Text="Remarks 1"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtHandremarks1" runat="server" TextMode="MultiLine" Width="45%"
                            Height="30px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel53" runat="server" Text="Remarks 2"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtHandremarks2" runat="server" TextMode="MultiLine" Width="45%"
                            Height="30px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel54" runat="server" Text="Remarks 3"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtHandremarks3" runat="server" TextMode="MultiLine" Width="45%"
                            Height="30px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel55" runat="server" Text="Remarks 4"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtHandremarks4" runat="server" TextMode="MultiLine" Width="45%"
                            Height="30px">
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
                        <telerik:RadLabel ID="RadLabel56" runat="server" Text="Assessment & Evaluation by Superintendent, Name"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtSuperintendentName" runat="server"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel57" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <eluc:Date ID="txtDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel58" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td colspan="5">
                        <telerik:RadTextBox ID="txtRemaks" runat="server" TextMode="MultiLine" Width="400px" Height="30px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel59" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td colspan="5">
                        <telerik:RadRadioButtonList ID="rblStatus" runat="server" Layout="Flow" Direction="Horizontal"
                            AutoPostBack="true" OnSelectedIndexChanged="rblStatus_SelectedIndexChanged">
                            <Items>
                                <telerik:ButtonListItem Value="1" Text="Approved" />
                                <telerik:ButtonListItem Value="2" Text="Conditionally Approved" />
                                <telerik:ButtonListItem Value="3" Text="Reject" />
                                <telerik:ButtonListItem Value="4" Text="Re-Interview" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
            </table>
            <eluc:Status ID="ucStatus" runat="server" />
            <eluc:Confirm ID="ucConfirmProposal" runat="server" OnConfirmMesage="btnProposal_Click" OKText="Yes" CancelText="No" />
        </telerik:RadAjaxPanel>

    </form>
</body>
</html>
