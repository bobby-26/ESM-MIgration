<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewLandTravelRequestGeneral.aspx.cs" Inherits="CrewLandTravelRequestGeneral" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TravelReason" Src="~/UserControls/UserControlLandTravelReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MUCCity" Src="~/UserControls/UserControlMultiColumnCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PhoneNumber" Src="../UserControls/UserControlMobileNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCompany" Src="~/UserControls/UserControlCompany.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Land Travel Request</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%=confirm.UniqueID %>", "");
                }
            }
             function sendemail(args) {
                if (args) {
                    __doPostBack("<%=sendemail.UniqueID %>", "");
                }
            }

        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmHotelRequestGeneral" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <asp:Button ID="confirm" runat="server" Text="confirm" OnClick="btnApprove_Click" />
             <asp:Button ID="sendemail" runat="server" Text="Send Email" OnClick="btnSendMail_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuLandTravelRequest" runat="server" OnTabStripCommand="MenuLandTravelRequest_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuRequest" runat="server" OnTabStripCommand="MenuRequest_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <table width="100%" cellpadding="2" cellspacing="2">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRequisitionNo" runat="server" Text="Reference No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReqNo" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="50%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCity" runat="server" Text="City"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MUCCity ID="ucCity" runat="server" CssClass="dropdown_mandatory" Width="50%" />
                    </td>
                    <td style="align-items: flex-start">
                        <span id="Span1" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                        <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="ucToolTipNW" Width="250px" ShowEvent="onmouseover"
                            RelativeTo="Element" Animation="Fade" TargetControlID="Span1" IsClientID="true"
                            HideEvent="ManualClose" Position="TopCenter" EnableRoundedCorners="true"
                            Text="">
                        </telerik:RadToolTip>
                        &nbsp;
                    </td>
                    <td></td>                   
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFromPlace" runat="server" Text="From Place"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFromPlace" runat="server" CssClass="input_mandatory" Width="50%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblToPlace" runat="server" Text="To Place"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtToPlace" runat="server" CssClass="input_mandatory" Width="50%"></telerik:RadTextBox>
                    </td>
                    <td></td>
                    <td></td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTravelDate" runat="server" Text="Travel Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucTravelDate" runat="server" CssClass="input_mandatory" TimeProperty="false" Width="50%" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTime" runat="server" Text="Time"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadMaskedTextBox runat="server" ID="txtFromTime" CssClass="input_mandatory" Width="20%" Mask="##:##"></telerik:RadMaskedTextBox>
                        <telerik:RadLabel ID="lblTo" runat="server" Text="to"></telerik:RadLabel>
                        <telerik:RadMaskedTextBox runat="server" ID="txtToTime" CssClass="input_mandatory" Width="20%" Mask="##:##"></telerik:RadMaskedTextBox>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTypeofTransport" runat="server" Text="Type of Transport"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTypeofTransport" runat="server" CssClass="input_mandatory" Width="50%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTypeofDuty" runat="server" Text="Type of Duty"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucTypeofDuty" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" HardTypeCode="247" Width="50%" />
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPackageType" runat="server" Text="Package Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlType" runat="server" Width="50%"
                            AppendDataBoundItems="true" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                            <Items>
                                <telerik:RadComboBoxItem Value="Dummy" Text="--Select--" />
                                <telerik:RadComboBoxItem Text="Meter Tariff" Value="1" />
                                <telerik:RadComboBoxItem Text="City Hourly Package" Value="2" />
                                <telerik:RadComboBoxItem Text="Out of City (Only Drop)" Value="3" />
                                <telerik:RadComboBoxItem Text="Out of City (Up & Down)" Value="4" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAgent" runat="server" Text="Agent"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListMaker">
                            <telerik:RadTextBox ID="txtVendorCode" runat="server" CssClass="input_mandatory" ReadOnly="false"
                                Width="15%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVenderName" runat="server" CssClass="input_mandatory" ReadOnly="false"
                                Width="35%">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" Style="cursor: pointer; vertical-align: top"
                                ID="ImgSupplierPickList" ToolTip="Select Mentor">                                
                                <span class="icon"><i class="fas fa-list-alt-picklist"></i></span>
                            </asp:LinkButton>

                            <telerik:RadTextBox ID="txtVendorId" runat="server" Width="10px"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNoOfPassengers" runat="server" Text="No. of Passengers"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtNoofPassengers" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="50%" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMobileNumber" runat="server" Text="Contact No"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:PhoneNumber ID="txtMobileNumber" IsMobileNumber="true" runat="server"  Width="50%" />
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOtherInfo" runat="server" Text="Any other information"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtOtherInfo" runat="server"  TextMode="MultiLine" Width="50%"
                            Rows="4">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                        <br />
                        <br />
                        <telerik:RadLabel ID="lblqAmount" runat="server" Text="Amount" Width="50%"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCurrency" runat="server" CssClass="readonlytextbox" Enabled="false" Width="50%"></telerik:RadTextBox>
                        <br />
                        <br />
                        <telerik:RadTextBox ID="txtAmount" runat="server" CssClass="readonlytextbox" Enabled="false" Width="50%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblcompany" runat="server" Text="Bill to Company"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlCompany ID="ddlcompany" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>' Width="50%"
                            CssClass="input_mandatory" runat="server" AppendDataBoundItems="true" />
                    </td>
                    <td colspan="2">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblinvoicecode" runat="server" Text="Invoice Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtInvoiceNumber" runat="server" CssClass="readonlytextbox" Enabled="false" Width="50%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblpayableamount" runat="server" Text="Payable Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtpayableamount" runat="server" CssClass="readonlytextbox" Enabled="false" Width="50%"></telerik:RadTextBox>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
            </table>
            <br />

            <div id="divReqDetail" runat="server">
                <telerik:RadLabel ID="lblRequisitionPassengerDetails" Font-Bold="true" runat="server" Text="Passenger Details"></telerik:RadLabel>
                <eluc:TabStrip ID="MenuPassengers" runat="server" OnTabStripCommand="MenuPassengers_TabStripCommand"></eluc:TabStrip>

                <telerik:RadGrid RenderMode="Lightweight" ID="gvPassengers" runat="server" EnableViewState="false"
                    AllowCustomPaging="true" AllowSorting="true" AllowPaging="false" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                    OnNeedDataSource="gvPassengers_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvPassengers_ItemDataBound" OnDeleteCommand="gvPassengers_DeleteCommand"
                    OnItemCommand="gvPassengers_ItemCommand" ShowFooter="false" AutoGenerateColumns="false" OnUpdateCommand="gvPassengers_UpdateCommand">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed">
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
                            <telerik:GridTemplateColumn HeaderText="Passenger Name" AllowSorting="false" ShowSortIcon="true">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblName" Width="150px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSENGERNAME") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblPassengerId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSENGERID") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Designation" AllowSorting="false" ShowSortIcon="true">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDesignation" Width="80px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESIGNATIONNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Budget Code" AllowSorting="false" ShowSortIcon="true">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGET") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <span id="spnPickListMainBudget">
                                        <telerik:RadTextBox ID="txtBudgetCode" runat="server" Width="60px"  Enabled="False" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'></telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtBudgetName" runat="server" Width="180px"  Enabled="False" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadTextBox>
                                        <asp:ImageButton ID="btnShowBudget" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                            ImageAlign="AbsMiddle" Text=".." OnClientClick="return showPickList('spnPickListMainBudget', 'codehelp1', '', 'Common/CommonPickListBudget.aspx?framename=ifMoreInfo', true); " />
                                        <telerik:RadTextBox ID="txtBudgetId" runat="server" Width="0px"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'></telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtBudgetgroupId" runat="server" Width="0px" ></telerik:RadTextBox>
                                    </span>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Payable by" AllowSorting="false" ShowSortIcon="true">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPayable" runat="server" Width="120px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYABLENAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Hard ID="ucPayable" runat="server" Width="120px" AppendDataBoundItems="true" HardTypeCode="185"  SelectedHard='<%# DataBinder.Eval(Container,"DataItem.FLDPAYABLE") %>' />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Cost" AllowSorting="false" ShowSortIcon="true">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCost" runat="server" Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOST") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Decimal ID="txtCost" runat="server" Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOST") %>'  IsPositive="true" DecimalPlace="2" />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Reason" AllowSorting="false" ShowSortIcon="true">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblReason" runat="server" Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREASON") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:TravelReason ID="ucTravelReason" runat="server" Width="80px" AppendDataBoundItems="true"  SelectedReason='<%# DataBinder.Eval(Container,"DataItem.FLDREASONFORTRAVEL") %>' />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Tentative Vessel" AllowSorting="false" ShowSortIcon="true">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblTentativeVessel" runat="server" Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTENTATIVEVESSELNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Vessel ID="ucTentativeVessel" runat="server" Width="120px" AppendDataBoundItems="true"  SelectedVessel='<%# DataBinder.Eval(Container,"DataItem.FLDTENTATIVEVESSELID") %>' />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Joined Vessel" AllowSorting="false" ShowSortIcon="true">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblJoinedVessel" runat="server" Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOINEDVESSELNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
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
            </div>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
