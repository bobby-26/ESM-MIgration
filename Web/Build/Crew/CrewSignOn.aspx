<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewSignOn.aspx.cs" Inherits="CrewSignOn" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCTSignOnReason" Src="~/UserControls/UserControlSignOnReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCRank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCVessel" Src="~/UserControls/UserControlCommonVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCSignOn" Src="~/UserControls/UserControlCrewOnboard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCPort" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCManager" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCPool" Src="~/UserControls/UserControlPool.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Sign-On</title>
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

        <script type="text/javascript" language="javascript">
            function checkTextAreaMaxLength(textBox, e, length) {

                var mLen = textBox["MaxLength"];
                if (null == mLen)
                    mLen = length;

                var maxLength = parseInt(mLen);
                if (!checkSpecialKeys(e)) {
                    if (textBox.value.length > maxLength - 1) {
                        if (window.event)//IE
                            e.returnValue = false;
                        else//Firefox
                            e.preventDefault();
                    }
                }
            }
            function checkSpecialKeys(e) {
                if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
                    return false;
                else
                    return true;
            }
        </script>
        <style type="text/css">
            .scrolpan {
                overflow-y: auto;
                height: 80%;
            }

            .checkRtl {
                direction: rtl;
            }

            .fon {
                font-size: small !important;
            }
        </style>
    </telerik:RadCodeBlock>


</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmSignOn" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="frmSignOn" runat="server">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>

        <asp:Button ID="confirm" runat="server" Text="confirm" OnClick="btnSignOn_Click" />
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <eluc:TabStrip ID="MenuSignOn" runat="server" OnTabStripCommand="CrewSignOn_TabStripCommand" Title="Sign On"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="90%" CssClass="scrolpan">
            <table cellpadding="1" cellspacing="1" width="99%">
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblEmployeeCode" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td style="width: 20%">
                        <telerik:RadTextBox ID="txtEmployeeCode" runat="server" CssClass="readonlytextbox" ReadOnly="True" Enabled="false" Width="240px"></telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblEmployeeName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 20%">
                        <telerik:RadTextBox ID="txtEmployeeName" runat="server" MaxLength="50" CssClass="readonlytextbox"
                            ReadOnly="True" Enabled="false" Width="240px">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblPayRank" runat="server" Text="Pay Rank"></telerik:RadLabel>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadTextBox ID="txtPayRank" runat="server" MaxLength="20" CssClass="readonlytextbox"
                            ReadOnly="True" Enabled="false" Width="240px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLastVessel" runat="server" Text="Last Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLastVessel" runat="server" MaxLength="20" CssClass="readonlytextbox"
                            ReadOnly="True" Enabled="false" Width="240px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSignedOff" Text="Signed Off" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSignedOff" runat="server" MaxLength="20" CssClass="readonlytextbox"
                            ReadOnly="True" Enabled="false" Width="240px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDOA" runat="server" Text="DOA"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtDOA" CssClass="readonlytextbox" ReadOnly="true" Width="240px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLastVesselRank" runat="server" Text="Last Vessel Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtLastVesselRank" CssClass="readonlytextbox" ReadOnly="true" Width="240px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastActivity" runat="server" Text="Last Activity"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtActivity" CssClass="readonlytextbox" ReadOnly="true" Width="240px"></telerik:RadTextBox>
                    </td>
                    <td><span id="Span1" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                        <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip1" Width="350px" ShowEvent="onmouseover" CssClass="fon"
                            RelativeTo="Element" Animation="Fade" TargetControlID="Span1" IsClientID="true"
                            HideEvent="ManualClose" Position="TopCenter" EnableRoundedCorners="true" ContentScrolling="Auto"
                            Text="*Note:<br/>Please select the reason for extra crew on board.<br/>If not an extra seafarer, Please select 'No'.">
                        </telerik:RadToolTip>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%;">
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td style="width: 20%;">
                        <eluc:UCVessel ID="ddlVessel" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            AutoPostBack="true" VesselsOnly="true" OnTextChangedEvent="MappedManagerPool" Enabled="false"
                            EntityType="VSL" ActiveVessels="true" Width="240px" />
                    </td>
                    <td style="width: 15%;">
                        <telerik:RadLabel ID="lblManager" runat="server" Text="Manager"></telerik:RadLabel>
                    </td>
                    <td style="width: 20%;">
                        <eluc:UCManager ID="ddlCompany" runat="server" AddressType="126" AppendDataBoundItems="true" CssClass="readonlytextbox"
                            Enabled="false" Width="240px" />
                    </td>
                    <td style="width: 15%;">
                        <telerik:RadLabel ID="lblPool" runat="server" Text="Pool"></telerik:RadLabel>
                    </td>
                    <td style="width: 15%;">
                        <eluc:UCPool ID="ddlPool" runat="server" AppendDataBoundItems="true" CssClass="readonlytextbox"
                            Enabled="false" Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCountry" runat="server" Text="Country"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Country ID="ucCountry" Width="240px" runat="server" AppendDataBoundItems="true" CssClass="input" AutoPostBack="true" OnTextChangedEvent="FilterSeaport" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSignOnPort" runat="server" Text="Sign-On Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UCPort ID="ddlPort" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblContract" runat="server" Text="Contract"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtContract" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="240px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UCRank ID="ddlRank" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" AutoPostBack="true" OnTextChangedEvent="MappedManagerPool" Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReasonOn" runat="server" Text="Reason On"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UCTSignOnReason ID="ddlSignOnReason" runat="server" AppendDataBoundItems="true"
                            CssClass="dropdown_mandatory" Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRelieveesName" runat="server" Text="Relievees Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UCSignOn ID="ddlSignOn" runat="server" AppendDataBoundItems="true" CssClass="input" Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSignOnDate" runat="server" Text="SignOn Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtSignOnDate" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                            OnTextChangedEvent="CalculateReliefDue" />
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblDuration" runat="server" Text="Duration (Months)"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtDuration" runat="server" CssClass="input_mandatory txtNumber"
                            OnTextChangedEvent="CalculateReliefDue" AutoPostBack="true" MaxLength="4" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReliefDue" runat="server" Text="Relief Due"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtReliefDueDate" runat="server" CssClass="input" AutoPostBack="true"
                            OnTextChangedEvent="CalculateReliefDue" />
                        <telerik:RadTextBox ID="txtSignOnOffId" runat="server" MaxLength="6" CssClass="input" Visible="false"></telerik:RadTextBox>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBeginToDTraveltoVSL" runat="server" Text="Begin ToD/Travel to VSL."></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtBeginToD" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblExtraCrewOnBoard" runat="server" Text="Extra Crew On Board"></telerik:RadLabel>
                    </td>
                    <td>

                        <telerik:RadComboBox DropDownPosition="Static" CssClass="dropdown_mandatory" ID="ddlExtraCrew" runat="server" EnableLoadOnDemand="True"
                            EmptyMessage="Type to select Extra Crew On Board" Filter="Contains" MarkFirstMatch="true" Width="240px">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--" />
                                <telerik:RadComboBoxItem Value="1" Text="Crew" />
                                <telerik:RadComboBoxItem Value="2" Text="Tech" />
                                <telerik:RadComboBoxItem Value="0" Text="No" />
                            </Items>
                        </telerik:RadComboBox>

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReasonforExtraCrew" runat="server" Text="Reason for extra crew"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox DropDownPosition="Static" CssClass="input" ID="ddlReasonExtraCrew" runat="server" EnableLoadOnDemand="True"
                            EmptyMessage="Type to select Reason for extra crew" Filter="Contains" MarkFirstMatch="true" Width="240px">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--" />
                                <telerik:RadComboBoxItem Value="1" Text="Dry Dock" />
                                <telerik:RadComboBoxItem Value="2" Text="Parallel" />
                                <telerik:RadComboBoxItem Value="3" Text="Maintenance" />
                                <telerik:RadComboBoxItem Value="4" Text="Vetting" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadCheckBox ID="chkBeginTravel" runat="server" AutoPostBack="true" CssClass="checkRtl" OnCheckedChanged="OnBeginTravelClick" Text="Allow begin travel and sign on on same date" TEXTPOSITION="LEFT" />
                    </td>
                    <td colspan="2">
                        <telerik:RadCheckBox ID="chkJoinWithPrevMed" runat="server" AutoPostBack="true" CssClass="checkRtl" Text="Joining with previous Pre-joining medicals" />

                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkAppraisalYN" runat="server" CssClass="checkRtl" Checked="true" Text="AppraisalYN" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td colspan="5">
                        <telerik:RadTextBox ID="txtSignonRemarks" runat="server" CssClass="input" TextMode="MultiLine"
                            MaxLength="200" onkeyDown="checkTextAreaMaxLength(this,event,'200');" Width="300px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand"></eluc:TabStrip>
            <eluc:Status ID="ucStatus" runat="server" />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewSignOn"  runat="server"
                AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvCrewSignOn_NeedDataSource" OnItemCommand="gvCrewSignOn_ItemCommand" EnableHeaderContextMenu="true" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDNAME">
                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <eluc:ToolTip ID="ucToolTipEmployee" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONREMARKS") %>' />
                                <telerik:RadLabel ID="lblSignOnId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONOFFID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>' CommandName="REDIRECT"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONRANK") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reason" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInActiveReason" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONREASON") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign-On" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignOnDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>

    </form>
</body>
</html>
