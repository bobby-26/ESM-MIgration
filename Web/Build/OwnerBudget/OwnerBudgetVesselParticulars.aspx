<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnerBudgetVesselParticulars.aspx.cs"
    Inherits="OwnerBudgetVesselParticulars" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="../UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="../UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="../UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="EngineType" Src="../UserControls/UserControlEngineType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="../UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Flag" Src="../UserControls/UserControlFlag.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Budget Vessel Particulars</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
           <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
      <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmBudgetProposal" runat="server">
                  <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />

    <telerik:RadAjaxPanel runat="server" ID="pnlBudgetProposal">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                        <telerik:RadButton ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />
                        <eluc:TabStrip ID="MenuParticulars" runat="server" TabStrip="true" OnTabStripCommand="MenuParticulars_TabStripCommand" />
                        <eluc:TabStrip ID="MenuAddEditParticulars" runat="server" OnTabStripCommand="MenuAddEditParticulars_TabStripCommand" />
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td width="10%" style="font-weight: 700">
                            <telerik:RadLabel ID="lblGENERAL" runat="server" Text="GENERAL"></telerik:RadLabel>
                        </td>
                        <td width="40%">
                        </td>
                        <td width="10%">
                        </td>
                        <td width="40%">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblProposalDate" runat="server" Text="Proposal Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucDate" runat="server" DatePicker="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblFlag" runat="server" Text="Flag"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Flag ID="ucFlag" runat="server"  AppendDataBoundItems="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblOwner" runat="server" Text="Owner"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:AddressType ID="ucNameofOwner" runat="server" AddressType="127" CssClass="input"
                                AppendDataBoundItems="true" />
                            <telerik:RadTextBox ID="txtNameOfOwner" runat="server" CssClass="input"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblDWT" runat="server" Text="DWT"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="ucDWT" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="ucVesselName" runat="server" CssClass="input" Enabled="false" AppendDataBoundItems="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblGRT" runat="server" Text="GRT"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="ucGRT" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:VesselType ID="ucVesselType" runat="server"  AppendDataBoundItems="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblHoldsTanks" runat="server" Text="Holds/Tanks"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="ucNoOfHoldsTank" DecimalPlace="0" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblYearBuilt" runat="server" Text="Year Built"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlYear" runat="server"  AutoPostBack="true" AppendDataBoundItems="true" Filter="Contains" EmptyMessage="Type to select" >
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblHatches" runat="server" Text="Hatches"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="ucNoOfHatches" DecimalPlace="0" runat="server" CssClass="input" />
                        </td>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblNewBuilding" runat="server" Text="New Building"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadRadioButtonList ID="rblstNewBuilding" runat="server" RepeatDirection="Horizontal" Width="100px" Direction="Horizontal">
                                    <Items>
                                    <telerik:ButtonListItem Text="Yes" Value="0" />
                                    <telerik:ButtonListItem Text="No" Value="1" Selected="True" />
                                        </Items>
                                </telerik:RadRadioButtonList>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblShipYard" runat="server" Text="Ship Yard"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:AddressType ID="ucShipYard" runat="server" AppendDataBoundItems="true" AddressType="133"
                                     />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblChinaYard" runat="server" Text="China Yard"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadRadioButtonList ID="rblstChinnaYard" runat="server" RepeatDirection="Horizontal" Direction="Horizontal">
                                    <Items>
                                    <telerik:ButtonListItem Text="Yes" Value="0" />
                                    <telerik:ButtonListItem  Text="No" Value="1" Selected="True"/>
                                        </Items>
                                </telerik:RadRadioButtonList>
                            </td>
                        </tr>
                    </tr>
                </table>
                <hr />
                <table cellpadding="2" cellspacing="1" width="100%">
                    <tr>
                        <td width="10%" style="font-weight: 700">
                            <telerik:RadLabel  ID="lblCREW" runat="server" Text="CREW"></telerik:RadLabel>
                        </td>
                        <td width="40%">
                        </td>
                        <td width="10%" style="font-weight: 700">
                            <telerik:RadLabel ID="lblCrewComplement" runat="server" Text="Crew Complement"></telerik:RadLabel>
                            <asp:ImageButton ID="cmdCrewComplement" runat="server" AlternateText="Owner Scale"
                                    ToolTip="Owner Scale" ImageUrl="<%$ PhoenixTheme:images/showlist.png %>" />
                            
                        </td>
                        <td width="10%">
                        </td>
                        <td width="15%" style="font-weight: 700">
                            <telerik:RadLabel ID="lblDurationofContract" runat="server" Text="Duration of Contract"></telerik:RadLabel>
                        </td>
                        <td width="15%">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblITFCover" runat="server" Text="ITF Cover"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadRadioButtonList ID="rblstITFCovered" runat="server" RepeatDirection="Horizontal" Direction="Horizontal">
                                <Items>
                                <telerik:ButtonListItem Text="Yes" Value="0" />
                                <telerik:ButtonListItem Text="No" Value="1" Selected="True"/>
                                    </Items>
                            </telerik:RadRadioButtonList>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblSeniorOfficers" runat="server" Text="Senior Officers"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="ucSOFFRequired" DecimalPlace="0" runat="server" CssClass="input" Enabled="false" />
                        </td>
                        <td>
                            <eluc:Number ID="ucSOFFContractPeriod" DecimalPlace="0" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblJapaneseOwner" runat="server" Text="Japanese Owner"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadRadioButtonList ID="rblstJapaneseOwner" runat="server" RepeatDirection="Horizontal" Direction="Horizontal">
                                <Items>
                                <telerik:ButtonListItem Text="Yes" Value="0" />
                                <telerik:ButtonListItem Text="No" Value="1" />
                                    </Items>
                            </telerik:RadRadioButtonList>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblJuniorOfficers" runat="server" Text="Junior Officers"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="ucJOFFRequired" DecimalPlace="0" runat="server" CssClass="input" Enabled="false"/>
                        </td>
                        <td>
                            <eluc:Number ID="ucJOFFContractPeriod" DecimalPlace="0" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                             <telerik:RadLabel ID="lblOverTimeHours" runat="server" Text="Over Time Hours"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="ucOverTimeHours" MaxLength="3" DecimalPlace="0" runat="server" CssClass="input" />Hrs/Mn
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblTrainees" runat="server" Text="Trainees"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="ucTRARequired" DecimalPlace="0" runat="server" CssClass="input" Enabled="false"/>
                        </td>
                        <td>
                            <eluc:Number ID="ucTRAContractPeriod" DecimalPlace="0" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblPortofRotation" runat="server" Text="Port of Rotation"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Quick ID="ucPortOfRotation" runat="server"  AppendDataBoundItems="true"
                             QuickTypeCode="115" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblRatings" runat="server" Text="Ratings"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="ucRATRequired" DecimalPlace="0" runat="server" CssClass="input" Enabled="false"/>
                        </td>
                        <td>
                            <eluc:Number ID="ucRATContractPeriod" DecimalPlace="0" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblNationality" runat="server" Text="Nationality"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Country ID="ucNationality" runat="server"  AppendDataBoundItems="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblVictuallingCost" runat="server" Text="Victualling Cost"></telerik:RadLabel> 
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtVictuallingCost" runat="server" CssClass="input" IsPositive="true" />/Man/Day
                        </td>
                    </tr>
                </table>
                <hr />
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td width="10%" style="font-weight: 700">
                            <telerik:RadLabel  ID="lblTECHNICAL" runat="server" Text="TECHNICAL"></telerik:RadLabel>
                        </td>
                        <td width="40%">
                        </td>
                        <td width="10%">
                        </td>
                        <td width="40%">
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblEngineType" runat="server" Text="Engine Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:EngineType ID="ucEngineType" runat="server"  AppendDataBoundItems="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblDockinginYear" runat="server" Text="Docking in Year"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="ucDockingInYear" runat="server" CssClass="input" />
                            <eluc:Number ID="ucDockingInYear2" runat="server" CssClass="input" />
                            <eluc:Number ID="ucDockingInYear3" runat="server" CssClass="input" />
                            <eluc:Number ID="ucDockingInYear4" runat="server" CssClass="input" />
                            <eluc:Number ID="ucDockingInYear5" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblBHP" runat="server" Text="BHP"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="ucBHP" runat="server" CssClass="input" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblVesselInspected" runat="server" Text="Vessel Inspected"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadRadioButtonList ID="rblstVesselInspected" runat="server" RepeatDirection="Horizontal" Direction="Horizontal">
                                <Items>
                                <telerik:ButtonListItem Text="Yes" Value="0" />
                                <telerik:ButtonListItem Text="No" Value="1" Selected="True"/>
                                    </Items>
                            </telerik:RadRadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblSailingDays" runat="server" Text="Sailing Days"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="ucNoOfSailingDay" DecimalPlace="0" runat="server" CssClass="input" />/Yr
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblMEStrokeType" runat="server" Text="M/E Stroke Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadRadioButtonList ID="rblstMEStrokeType" runat="server" RepeatDirection="Horizontal" Direction="Horizontal">
                                <Items>
                                <telerik:ButtonListItem Text="2 Stroke" Value="0" Selected="True"/>
                                <telerik:ButtonListItem Text="4 Stroke" Value="1" />
                                    </Items>
                            </telerik:RadRadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <telerik:RadLabel ID="lblMainEngineRunAt" runat="server" Text="Main Engine Run At"></telerik:RadLabel>
                            &nbsp
                            <eluc:Number ID="ucEngineRunAT" MaxLength="5" DecimalPlace="2" runat="server" CssClass="input" />
                            <telerik:RadLabel ID="lblMCR" runat="server" Text="%MCR"></telerik:RadLabel>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        
                    </tr>
                    <tr>
                        
                    </tr>
                    <tr>
                        
                    </tr>
                </table>
                <hr />
                 <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td width="10%">
                            <telerik:RadLabel ID="lblManagementType" runat="server" Text="Management  Type"></telerik:RadLabel>
                        </td>
                        <td width="40%">
                            <telerik:RadComboBox ID ="ddlManagementType" runat="server" Filter="Contains" EmptyMessage="Type to select" >
                                <Items>
                                <telerik:RadComboBoxItem Text="Full" Value="1"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Technical" Value="2"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Crew" Value="3"></telerik:RadComboBoxItem>
                                    </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td width="10%">
                            <telerik:RadLabel ID="lblSupdtAttendance" runat="server" Text="Supdt Attendance"></telerik:RadLabel>
                        </td>
                        <td width="40%">
                            <eluc:Number ID="ucSupdtAttendance" IsInteger="true" IsPositive="true" runat="server" CssClass="input" />Days/Yr
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <telerik:RadGrid RenderMode="Lightweight" ID="gvContactType" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3"  ShowFooter="false" AllowSorting="true" AllowCustomPaging="true"  AllowPaging="true" GroupingEnabled="false" EnableHeaderContextMenu="true"
                        ShowHeader="true" EnableViewState="false" OnSortCommand="gvContactType_SortCommand" OnNeedDataSource="gvContactType_NeedDataSource"
                        OnItemCommand="gvContactType_ItemCommand" 
                        OnItemDataBound="gvContactType_ItemDataBound" 
                         OnUpdateCommand="gvContactType_UpdateCommand"
                        OnDeleteCommand="gvContactType_DeleteCommand">
                     
                     
                                 <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" >
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Seq">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblExpenses" runat="server" Text="Expenses"></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                      <telerik:RadLabel ID="lblAllowanceId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSALEXPENSESMAPPING") %>' ></telerik:RadLabel>
                                      <telerik:RadLabel ID="lblAllowanceName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUICKNAME") %>' ></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="BHP" >
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <FooterStyle HorizontalAlign="Right" />
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblYesNo" runat="server" Text="BHP"></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblYesNO" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.YESNO") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                   <telerik:RadRadioButtonList ID="rblstYesNo" runat="server" RepeatDirection="Horizontal" Direction="Horizontal" AutoPostBack="false">
                                       <Items>
                                        <telerik:ButtonListItem Text="Yes" Value="0"  />
                                        <telerik:ButtonListItem Text="No" Value="1" Selected="true" />
                                           </Items>
                                    </telerik:RadRadioButtonList>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                                       <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                            <HeaderStyle Width="88px" />
                            <ItemStyle Width="20px" Wrap="false" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                             
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                                  
                        </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                                 <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
                    </telerik:RadGrid>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
                <eluc:Status runat="server" ID="ucStatus" />
<%--            </div>--%>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
