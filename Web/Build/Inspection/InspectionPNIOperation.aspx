<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionPNIOperation.aspx.cs"
    Inherits="InspectionPNIOperation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PNI Operation</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
            <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <div id="divmenustrip" runat="server">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <eluc:UserControlStatus ID="ucStatus" runat="server" />

                    <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />
                    <eluc:TabStrip ID="MenuPNIGeneral" TabStrip="true" runat="server" OnTabStripCommand="MenuPNIGeneral_TabStripCommand"></eluc:TabStrip>
                    <eluc:TabStrip ID="PNIListMain" runat="server" OnTabStripCommand="PNIListMain_TabStripCommand"></eluc:TabStrip>

                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblInspectionPNI" width="100%">
                        <tr>
                            <td style="width: 15%">
                                <telerik:RadLabel ID="lblCaseNo" runat="server" Text="Case No"></telerik:RadLabel>
                            </td>
                            <td style="width: 35%">
                                <telerik:RadTextBox ID="txtcaseNo" CssClass="readonlytextbox" Enabled="false" runat="server"
                                    MaxLength="200" Width="35%">
                                </telerik:RadTextBox>
                            </td>
                            <td style="width: 15%">
                                <telerik:RadLabel ID="lblPNIClubCaseNo" runat="server" Text="PNI Club Case No"></telerik:RadLabel>
                            </td>
                            <td style="width: 35%">
                                <telerik:RadTextBox ID="txtPNIClubCaseNo" CssClass="input" runat="server" MaxLength="200"
                                    Width="35%">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems="true" CssClass="input_mandatory"
                                    VesselsOnly="true" AutoPostBack="true" OnTextChangedEvent="ucVessel_TextChanged"
                                    Width="35%" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblInformedPNI" runat="server" Text="Informed to PNI Club On"></telerik:RadLabel>
                            </td>
                            <td style="width: 35%">
                                <eluc:Date ID="ucInformedPNI" runat="server" CssClass="input" DatePicker="true" AutoPostBack="true"
                                    Width="35%" />
                                <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgPPClip" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPort" runat="server" Text="Port"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Number ID="txtDaysLost" runat="server" ReadOnly="true" Enabled="false" CssClass="readonlytextbox"
                                    IsInteger="true" IsPositive="true" Visible="false" />
                                <eluc:MultiPort ID="ucPort" runat="server" CssClass="input_mandatory" Width="300px" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPOVessel" runat="server" Text="PO of the Vessel"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtPOVessel" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="35%">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblSingaporePIC" runat="server" Text="Singapore PIC"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtSingaporePIC" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="35%">
                                </telerik:RadTextBox>
                            </td>
                            <td style="width: 20%">
                                <telerik:RadLabel ID="lblPortAgent" runat="server" Text="Port Agent"></telerik:RadLabel>
                            </td>
                            <td style="width: 30%">
                                <span id="spnPickListAgent">
                                    <telerik:RadTextBox ID="txtAgentNumber" runat="server" Width="60px" CssClass="input_mandatory"
                                        Enabled="False">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtAgentName" runat="server" Width="180px" CssClass="input_mandatory"
                                        Enabled="False">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton runat="server" ID="cmdShowAgent" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" OnClientClick="return showPickList('spnPickListAgent', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=1255', true);"
                                        Text=".." />
                                    <telerik:RadTextBox ID="txtAgent" runat="server" Width="1px" CssClass="input_mandatory"></telerik:RadTextBox>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCrewName" runat="server" Text="Crew Name"></telerik:RadLabel>
                            </td>
                            <td>
                                <span id="spnCrewInCharge">
                                    <telerik:RadTextBox ID="txtCrewName" runat="server" CssClass="input_mandatory" Enabled="false"
                                        MaxLength="50" Width="35%">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtCrewRank" runat="server" CssClass="input_mandatory" Enabled="false"
                                        MaxLength="50" Width="25%">
                                    </telerik:RadTextBox>
                                    <img runat="server" id="imgShowCrewInCharge" style="cursor: pointer; vertical-align: top"
                                        src="<%$ PhoenixTheme:images/picklist.png %>" />
                                    <telerik:RadTextBox ID="txtCrewId" runat="server" CssClass="input" MaxLength="20" Width="10px"></telerik:RadTextBox>
                                </span>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblServiceYears" runat="server" Text="Service Years"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Number ID="txtServiceYears" runat="server" CssClass="readonlytextbox" IsInteger="true"
                                    Width="35%" IsPositive="true" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%">
                                <telerik:RadLabel ID="lblIllnessInjuryDate" runat="server" Text="Illness/Injury Date"></telerik:RadLabel>
                            </td>
                            <td style="width: 35%">
                                <eluc:Date ID="ucInjuryDate" runat="server" CssClass="input_mandatory" DatePicker="true"
                                    AutoPostBack="true" Width="35%" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblCaseOpenedDate" runat="Server" Text="Case Opened Date"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="ucCaseOpenedDate" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                                    Width="35%" />

                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCaseOpenedBy" runat="server" Text="Case Opened by"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtCaseOpenedby" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="35%">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lbldoctorVistDate" runat="server" Text=" Doctor Visit Date"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="ucDoctorVisitDate" runat="server" CssClass="input" DatePicker="true"
                                    Width="35%" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblTypeofCase" runat="server" Text="Type of Case"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Hard ID="ucTypeofcase" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                    HardTypeCode="174" AutoPostBack="true" OnTextChangedEvent="ucTypeofcase_Changed"
                                    Width="35%" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblTypeofInjury" runat="server" Text="Type of Injury"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Quick runat="server" ID="ucInjuryType" AppendDataBoundItems="true" CssClass="input"
                                    Width="35%" Enabled="false" QuickTypeCode="69" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPartsofBodyInjured" runat="Server" Text="Parts of Body Injured"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Quick runat="server" ID="ucPartsinjured" AppendDataBoundItems="true" CssClass="input"
                                    Width="35%" Enabled="false" QuickTypeCode="68" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblCategoryOfWorkInjury" runat="server" Text="Category of Work Injury"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Quick runat="server" ID="ucInjuryCategory" AppendDataBoundItems="true" CssClass="input"
                                    Enabled="false" QuickTypeCode="70" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <font color="blue" size="1">
                                    <telerik:RadLabel ID="lblThisfieldisMandatory" runat="server" Text="This field is Mandatory if 'Type of Case' is 'Injury'."></telerik:RadLabel>
                                </font>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCrewHospitalized" runat="server" Text="Crew Hospitalized"></telerik:RadLabel>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkHospital" runat="server" Text="" AutoPostBack="true" OnCheckedChanged="Hospitalizedfn" />
                            </td>
                            <td style="width: 15%">
                                <telerik:RadLabel ID="lblMedicalFitDate" runat="server" Text="Medical Fit Date"></telerik:RadLabel>
                            </td>
                            <td style="width: 35%">
                                <eluc:Date ID="ucMedicalFitDate" runat="server" CssClass="input" ReadOnly="true"
                                    Width="35%" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblHospitalName" runat="server" Text="Hospital Name"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtHospitalName" runat="server" CssClass="input" Enabled="false"
                                    Width="35%">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblHospitalContactNo" runat="server" Text="Hospital Contact No"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtHospitalContactNo" runat="server" CssClass="input" Enabled="false"
                                    Width="35%">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblComprehensiveDescription" runat="server" Text="Comprehensive Description"></telerik:RadLabel>
                            </td>
                            <td colspan="3">
                                <telerik:RadTextBox ID="txtDescription" runat="server" CssClass="input" Height="75px" TextMode="MultiLine"
                                    Width="80%">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divgridview" runat="server">
                    <b>
                        <telerik:RadLabel ID="Label1" Text="Vessel Medical Reports" runat="server"></telerik:RadLabel>
                    </b>

                    <eluc:TabStrip ID="MenuDeficiency" runat="server" OnTabStripCommand="Deficiency_TabStripCommand"></eluc:TabStrip>

                    <div id="divGrid" style="position: relative; z-index: 1; width: 100%;" runat="server">
                  
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvDeficiency" runat="server"  AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                            CellSpacing="0" GridLines="None" OnNeedDataSource="gvDeficiency_NeedDataSource"
                            OnItemCommand="gvDeficiency_ItemCommand"
                            OnItemDataBound="gvDeficiency_ItemDataBound1"
                            GroupingEnabled="false" EnableHeaderContextMenu="true"

                            AutoGenerateColumns="false">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <MasterTableView DataKeyNames="FLDPNIMEDICALCASEID" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" TableLayout="Fixed">
                                <HeaderStyle Width="102px" />
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Reference Number">
                                        <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                     
                                        <itemtemplate>
                                        <telerik:RadLabel ID="lblPNIMedicalCaseId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPNIMEDICALCASEID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                        <asp:LinkButton ID="lnkRefNumber" runat="server" CommandName="Select" CommandArgument='<%# Container.DataSetIndex %>'
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENO") %>' Visible="false"></asp:LinkButton>
                                        <telerik:RadLabel ID="lblRefNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENO") %>'></telerik:RadLabel>
                                    </itemtemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Vessel">
                                        <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                    
                                        <itemtemplate>
                                        <asp:LinkButton ID="lnkVessel" runat="server" CommandName="Select" CommandArgument='<%# Container.DataSetIndex %>'
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>' Visible="false"></asp:LinkButton>
                                        <telerik:RadLabel ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                    </itemtemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Crew Name">
                                        <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                      
                                        <itemtemplate>
                                        <telerik:RadLabel ID="lblCrewName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWNAME") %>'></telerik:RadLabel>
                                    </itemtemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Rank">
                                        <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                     
                                        <itemtemplate>
                                        <telerik:RadLabel ID="lblCrewRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWRANK") %>'></telerik:RadLabel>
                                    </itemtemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Illness Date">
                                        <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                      
                                        <itemtemplate>
                                        <telerik:RadLabel ID="lblIllnessDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATEOFILLNESS"))%>'></telerik:RadLabel>
                                    </itemtemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Doctor Visit Date">
                                        <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                   
                                        <itemtemplate>
                                        <telerik:RadLabel ID="lblDoctorVisitDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDOCTORVISITDATE"))%>'></telerik:RadLabel>
                                    </itemtemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Description">
                                        <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                    
                                        <itemtemplate>
                                        <eluc:ToolTip ID="ucToolTipAddress" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDILLNESSDESCRIPTION") %>' />
                                        <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDILLNESSDESCRIPTION") %>'
                                            Visible="false"></telerik:RadLabel>
                                        <img id="imgRemarks" runat="server" style="cursor: hand" src="<%$ PhoenixTheme:images/te_view.png %>"
                                            onmousedown="javascript:closeMoreInformation()" />
                                    </itemtemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Action">
                                        <headerstyle horizontalalign="Center" verticalalign="Middle"></headerstyle>
                                   
                                        <itemstyle wrap="False" horizontalalign="Center" width="100px"></itemstyle>
                                        <itemtemplate>
                                        <asp:LinkButton runat="server" AlternateText="Sickness Report" 
                                            CommandName="SICKNESSREPORT" CommandArgument='<%# Container.DataSetIndex %>'
                                            ID="cmdSicknessReport" ToolTip="Sickness Report">
                                            <span class="icon"><i class="fas fa-chart-bar"></i></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" AlternateText="PNI Checklist" CommandName="CHECKLIST"
                                            ID="cmdChkList" ToolTip="PNI Checklist">
                                            <span class="icon"><i class="fas fa-tasks-checklist"></i></span>
                                        </asp:LinkButton>
                                        <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                            CommandName="ATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAttachment"
                                            ToolTip="Medical Unfit Report"></asp:ImageButton>
                                        <asp:ImageButton runat="server" AlternateText="No Attachment" ImageUrl="<%$ PhoenixTheme:images/no-attachment.png %>"
                                            CommandName="NOATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdNoAttachment"
                                            ToolTip="Medical Unfit Report"></asp:ImageButton>
                                    </itemtemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight=""/>
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </div>
                    
                    <td colspan="2">
                        <font color="blue" size="1">
                            <telerik:RadLabel ID="lblKindlyattachall" runat="server" Text="Note:Kindly attach all other documents in the relevant heads in the 'Check List' screen "></telerik:RadLabel>
                        </font>
                    </td>
                </div>
            </div>
            <eluc:Confirm ID="ucConfirmDuplicate" runat="server" OnConfirmMesage="btnConfirmDuplicate_Click"
                OKText="Yes" CancelText="No" Visible="false" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
