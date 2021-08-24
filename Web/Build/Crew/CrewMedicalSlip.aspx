<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewMedicalSlip.aspx.cs"
    Inherits="CrewMedicalSlip" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Clinic" Src="~/UserControls/UserControlClinic.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Medical</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function btnconfirm(args) {
                if (args) {
                    __doPostBack("<%=btnconfirm.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewList" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <eluc:TabStrip ID="CrewMedical" runat="server" OnTabStripCommand="CrewMedical_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="150%">
            <asp:Button ID="btnconfirm" runat="server" Text="btnconfirm" OnClick="confirm_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td colspan="3">
                        <telerik:RadLabel ID="lblguidanceText" runat="server" Text=" Note: Liver Function Test(LFT) required for seafarers signed off from all oil chemical
                            vessels">
                        </telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtName" ReadOnly="true"
                            Width="150px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRankName" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtRank" ReadOnly="true" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFileNo" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" ReadOnly="true" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCDCNumber" runat="server" Text="CDC Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtCDC" ReadOnly="true" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbldob" runat="server" Text="D.O.B."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtDOB" ReadOnly="true" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPlaceofBirth" runat="server" Text="Place of Birth"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtPlaceofbirth" ReadOnly="true" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <br />
            <hr />
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRequest" runat="server" Text="Request Type"></telerik:RadLabel>
                    </td>
                    <td>                      
                        <telerik:RadRadioButtonList ID="rblRequest" runat="server" Layout="Flow" Columns="2" Direction="Horizontal"
                            AutoPostBack="true" OnSelectedIndexChanged="rblRequest_SelectedIndexChanged">
                            <Items>
                                <telerik:ButtonListItem Value="1" Text="Full medical request" />
                                <telerik:ButtonListItem Value="2" Text="LFT request" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>                       
                        <telerik:RadComboBox ID="ucVessel" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" Width="52%"
                            AutoPostBack="true" OnTextChanged="VesselChanged" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td width="20%">
                        <telerik:RadLabel ID="lblZone" runat="server" Text="Zone"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Zone ID="ddlZone" runat="server" AppendDataBoundItems="true" Width="80%"
                            AutoPostBack="true" OnTextChangedEvent="ClinicAddress" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblClinic" runat="server" Text="Clinic"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Clinic ID="ddlClinic" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" Width="52%" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAppointment" runat="server" Text="Appointment"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory" />
                        <telerik:RadMaskedTextBox runat="server" ID="txtTime" CssClass="input_mandatory" Width="50px" Mask="##:##" ></telerik:RadMaskedTextBox>
                        <telerik:RadLabel ID="lblhrs" runat="server" Text="(hrs)"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBudgetCode" runat="server" Text="Budget Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBudgetCode" runat="server" ReadOnly="true"
                            Width="20%">
                        </telerik:RadTextBox>                      
                        <telerik:RadComboBox ID="ddlBudgetCode" runat="server" CssClass="dropdown_mandatory"  Width="32%"
                            AppendDataBoundItems="true" AutoPostBack="true" OnTextChanged="OnBudgetChange" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselAccount" runat="server" Text="Vessel Account"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlAccountDetails" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true"
                            OnDataBound="ddlAccountDetails_DataBound" DataTextField="FLDVESSELACCOUNTNAME" OnTextChanged="ddlAccountDetails_TextChanged"
                            DataValueField="FLDACCOUNTID" Width="80%" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOwnerBudgetCode" runat="server" Text="Owner Budget Code"></telerik:RadLabel>
                    </td>
                    <td>                    
                         <telerik:RadComboBox ID="ddlOwnerBudgetCode" runat="server" CssClass="dropdown_mandatory" Width="52%"
                            AppendDataBoundItems="false" AutoPostBack="true" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPaymentBy" runat="server" Text="Payment By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblFee" runat="server" Layout="Flow" Columns="2" Direction="Horizontal">
                            <DataBindings DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE" />
                        </telerik:RadRadioButtonList>   
                                       
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadLabel ID="lbl1" runat="server" Text="Please examine the following persons for M.V/M.T"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblVesselName" runat="server" Font-Bold="true"></telerik:RadLabel>
                        <telerik:RadLabel ID="lbl2" runat="server" Text="as per the requirement below"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td width="15%">
                        <b>
                            <telerik:RadLabel ID="lblMedicalFlag" runat="server" Text="Medical Examination for Flags"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>                    
                        <telerik:RadCheckBoxList ID="cblFlagMedical" runat="server" RepeatDirection="Horizontal" Columns="4">
                            <Databindings DataTextField="FLDFLAGNAME" DataValueField="FLDCOUNTRYCODE" />
                        </telerik:RadCheckBoxList>

                    </td>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblpreexamination" runat="server" Text="Pre Employment Medical Examination"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>                      
                         <telerik:RadCheckBoxList ID="cblPreMedical" runat="server" RepeatDirection="Horizontal" Columns="3">
                            <Databindings DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE" />
                        </telerik:RadCheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblTest" runat="server" Text="Test"></telerik:RadLabel>
                        </b>
                    </td>
                    <td colspan="3">                    
                          <telerik:RadCheckBoxList ID="cblMedicalTest" runat="server" RepeatDirection="Horizontal" Columns="4">
                            <Databindings DataTextField="FLDNAMEOFMEDICAL" DataValueField="FLDDOCUMENTMEDICALID" />
                        </telerik:RadCheckBoxList>

                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblVaccination" runat="server" Text="Vaccination"></telerik:RadLabel>
                        </b>
                    </td>
                    <td colspan="4">                       
                         <telerik:RadCheckBoxList ID="cblVaccination" runat="server" RepeatDirection="Horizontal" Columns="6">
                            <Databindings DataTextField="FLDNAMEOFMEDICAL" DataValueField="FLDDOCUMENTMEDICALID" />
                        </telerik:RadCheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblDeclaration" runat="server" Text="Declaration"></telerik:RadLabel>
                        </b>
                    </td>
                    <td colspan="4">                        
                        <telerik:RadCheckBoxList ID="cblDeclaration" runat="server" RepeatDirection="Horizontal" Columns="6">
                            <Databindings DataTextField="FLDNAMEOFDECLARATION" DataValueField="FLDMEDICALDECLARATIONID" />
                        </telerik:RadCheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblOther" runat="server" Text="Others(Please Specify)"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtOthers" TextMode="MultiLine" runat="server"
                            Height="30px" Width="360px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvReq" runat="server" Height="40%" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvReq_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvReq_ItemDataBound"
                OnItemCommand="gvReq_ItemCommand" ShowFooter="false" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle />
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Request No." AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRefNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Appointment" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAppointmentDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDAPPOINTMENTDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name/ File No." AllowSorting="true" DataField="FLDNAME" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                <br /> /
                                <telerik:RadLabel ID="lblFileno" runat="server" Text= '<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Tentative Vessel" AllowSorting="true" DataField="FLDVESSELCODE" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTentativeVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELCODE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRequestId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblStatus" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTTYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Clinic" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblClinic" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLINICADDRESSNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Family Member" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFamilyMember" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAFARERNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Attended" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAttended" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDATTENDEDYN").ToString() == "1" ? "Yes" : "No"%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Payment By" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPaymentBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROFESSIONALFEE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Created By" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCreatedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDBY") %>'></telerik:RadLabel>
                                <br />
                                On
                              <telerik:RadLabel ID="lblCreatedDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatusName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipStatus" runat="server" Text='<%# "Cancelled By : " + DataBinder.Eval(Container,"DataItem.FLDM0DIFIEDBY") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit" ToolTip="Edit" CommandName="SELECT" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Medical Request" ID="cmdMedical" CommandName="MEDICALREQUEST" ToolTip="Medical Request" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-briefcase-medical"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel Request" ID="cmdCancelRequest" ToolTip="Cancel Request" CommandName="CANCELREQUEST" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="false" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
