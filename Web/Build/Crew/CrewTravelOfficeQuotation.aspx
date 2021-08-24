<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelOfficeQuotation.aspx.cs" Inherits="CrewTravelOfficeQuotation" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TravelReason" Src="~/UserControls/UserControlTravelReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Agent Details </title>
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
        <style>
            .tooltipHeight {
                height: 130px;
                padding: 0;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewTravelQuotationAgentDetail" runat="server" autocomplete="off">
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <%--   <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvAgent">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvAgent"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>--%>
        <eluc:TabStrip ID="MenuAgent" runat="server" TabStrip="true" OnTabStripCommand="MenuAgent_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="88%">
            <eluc:Status ID="ucStatus" runat="server" />
            <asp:Button ID="btnconfirm" runat="server" Text="btnconfirm" OnClick="btn_approve" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:ConfirmMessage ID="ucConfirm" runat="server" Text="" Visible="false" />

            <eluc:TabStrip ID="Menuapprove" runat="server" OnTabStripCommand="Menuapprove_TabStripCommand"></eluc:TabStrip>
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselName" runat="server" Text=" Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVessel" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="80%">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselAccount" runat="server" Text="Vessel Account"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlAccountDetails" runat="server" CssClass="dropdown_mandatory"
                            OnDataBound="ddlAccountDetails_DataBound" DataTextField="FLDVESSELACCOUNTNAME" DataValueField="FLDACCOUNTID"
                            EnableLoadOnDemand="True" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCrewChangePort" runat="server">Crew Change Port</telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MultiPort ID="ucport" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" Width="300px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateofCrewChange" runat="server">Crew Change Date</telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDateOfCrewChange" runat="server" CssClass="input_mandatory" />
                    </td>
                </tr>
            </table>
            <telerik:RadLabel ID="RadLabel1" runat="server" Font-Bold="true" Text="Passenger List"></telerik:RadLabel>


            <telerik:RadGrid RenderMode="Lightweight" ID="gvCCT" runat="server" AllowCustomPaging="true" AllowSorting="true" RetainExpandStateOnRebind="true" OnUpdateCommand="gvCCT_UpdateCommand"
                CellSpacing="0" GridLines="None" OnItemCommand="gvCCT_ItemCommand" OnNeedDataSource="gvCCT_NeedDataSource" OnDetailTableDataBind="gvCCT_DetailTableDataBind"
                OnItemDataBound="gvCCT_ItemDataBound" EnableViewState="true" GroupingEnabled="false" EnableHeaderContextMenu="true" AllowPaging="true" OnDeleteCommand="gvCCT_DeleteCommand">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" EnableHierarchyExpandAll="true" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"
                    CommandItemDisplay="Top" DataKeyNames="FLDREQUESTID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <DetailTables>
                        <telerik:GridTableView Name="gvBreakUp" Width="100%" DataKeyNames="FLDREQUESTID,FLDEMPLOYEEID" AllowPaging="false" EditMode="InPlace" ShowFooter="true" AutoGenerateColumns="false">
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="S.No." HeaderStyle-Width="45px" Visible="false">
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <telerik:RadLabel runat="server" ID="lblTravelReqNo" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUISITIONNO") %>'></telerik:RadLabel>
                                        <%#DataBinder.Eval(Container, "DataItem.FLDSERIALNO")%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Origin" HeaderStyle-Width="125px">
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblEmployeeId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="lblTravelRequestId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="lblOnSignerYN" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDONSIGNERYN") %>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="lblBreakUpId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDBREAKUPID") %>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="lblIsEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDISEDIT")%>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                        <%#DataBinder.Eval(Container, "DataItem.FLDORIGINNAME")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblEmployeeIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="lblTravelRequestIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="lblVesselIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="lblOnSignerYNEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDONSIGNERYN") %>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="lblBreakUpIdEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDBREAKUPID") %>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                        <span id="spnPickListOriginOldbreakup">
                                            <telerik:RadTextBox ID="txtOriginNameOldBreakup" runat="server" Width="80%" Enabled="False"
                                                CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGINNAME") %>'>
                                            </telerik:RadTextBox>
                                            <asp:ImageButton ID="btnShowOriginoldbreakup" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                                ImageAlign="Top" Text=".." CommandName="BUDGETCODE"
                                                OnClientClick="return showPickList('spnPickListOriginOldbreakup', 'codehelp1', '', '../Common/CommonPickListCity.aspx', true); " />
                                            <telerik:RadTextBox ID="txtOriginIdOldBreakup" runat="server" Width="0px" CssClass="hidden"
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGINID") %>'>
                                            </telerik:RadTextBox>
                                        </span>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <span id="spnPickListOriginbreakupAdd">
                                            <telerik:RadTextBox ID="txtOriginNameBreakupAdd" runat="server" Width="80%" Enabled="False"
                                                CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGINNAME") %>'>
                                            </telerik:RadTextBox>
                                            <asp:ImageButton ID="btnShowOriginbreakupAdd" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                                ImageAlign="Top" Text=".." CommandName="BUDGETCODE"
                                                OnClientClick="return showPickList('spnPickListOriginbreakupAdd', 'codehelp1', '', '../Common/CommonPickListCity.aspx', true); " />
                                            <telerik:RadTextBox ID="txtOriginIdBreakupAdd" runat="server" Width="0px" CssClass="hidden"
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGINID") %>'>
                                            </telerik:RadTextBox>
                                        </span>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Destination" HeaderStyle-Width="125px">
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemTemplate>
                                        <%#DataBinder.Eval(Container, "DataItem.FLDDESTINATIONNAME")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <span id="spnPickListDestinationOldbreakup">
                                            <telerik:RadTextBox ID="txtDestinationNameOldBreakup" runat="server" Width="80%" Enabled="False"
                                                CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONNAME") %>'>
                                            </telerik:RadTextBox>
                                            <asp:ImageButton ID="btnShowDestinationOldbreakup" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                                ImageAlign="Top" Text=".." CommandName="BUDGETCODE"
                                                OnClientClick="return showPickList('spnPickListDestinationOldbreakup', 'codehelp1', '', '../Common/CommonPickListCity.aspx', true); " />
                                            <telerik:RadTextBox ID="txtDestinationIdOldBreakup" runat="server" Width="0px" CssClass="hidden"
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONID") %>'>
                                            </telerik:RadTextBox>
                                        </span>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <span id="spnPickListDestinationbreakupAdd">
                                            <telerik:RadTextBox ID="txtDestinationNameBreakupAdd" runat="server" Width="80%" Enabled="False"
                                                CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONNAME") %>'>
                                            </telerik:RadTextBox>
                                            <asp:ImageButton ID="btnShowDestinationbreakupAdd" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                                ImageAlign="Top" Text=".." CommandName="BUDGETCODE"
                                                OnClientClick="return showPickList('spnPickListDestinationbreakupAdd', 'codehelp1', '', '../Common/CommonPickListCity.aspx', true); " />
                                            <telerik:RadTextBox ID="txtDestinationIdBreakupAdd" runat="server" Width="0px" CssClass="hidden"
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONID") %>'>
                                            </telerik:RadTextBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Departure Date" HeaderStyle-Width="150px">
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDepartureDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDEPARTUREDATE", "{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="LBLDEPARTUREAMPM" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREAMPM") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Date runat="server" ID="txtDepartureDateOld" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE","{0:dd/MM/yyyy}") %>'></eluc:Date>
                                        <telerik:RadComboBox ID="ddldepartureampmold" runat="server" CssClass="dropdown_mandatory" Width="45px">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="AM" Value="1" />
                                                <telerik:RadComboBoxItem Text="PM" Value="2" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Date runat="server" ID="txtDepartureDateAdd" CssClass="input_mandatory"></eluc:Date>
                                        <telerik:RadComboBox ID="ddldepartureampmAdd" runat="server" CssClass="dropdown_mandatory" Width="45px">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="AM" Value="1" />
                                                <telerik:RadComboBoxItem Text="PM" Value="2" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Arrival Date" HeaderStyle-Width="150px">
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblArrivalDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE", "{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblARRIBALAMPM" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDARRIVALAMPM") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Date runat="server" ID="txtArrivalDate" CssClass="input" Text='<%# DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE","{0:dd/MM/yyyy}") %>'></eluc:Date>
                                        <telerik:RadComboBox ID="ddlarrivalampm" runat="server" CssClass="input" Width="45px">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="--" Value="DUMMY" />
                                                <telerik:RadComboBoxItem Text="AM" Value="1" />
                                                <telerik:RadComboBoxItem Text="PM" Value="2" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Date runat="server" ID="txtArrivalDateAdd" CssClass="input" Text='<%# DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE","{0:dd/MM/yyyy}") %>'></eluc:Date>
                                        <telerik:RadComboBox ID="ddlarrivalampmAdd" runat="server" CssClass="input" Width="45px">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="--" Value="DUMMY" />
                                                <telerik:RadComboBoxItem Text="AM" Value="1" />
                                                <telerik:RadComboBoxItem Text="PM" Value="2" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Purpose" HeaderStyle-Width="100px">
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel runat="server" ID="lblPurpose" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPURPOSENAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:TravelReason ID="ucPurposeOld" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="100%"
                                            ReasonList="<%# PhoenixCrewTravelRequest.ListTravelReason(null) %>" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:TravelReason ID="ucPurposeAdd" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="100%"
                                            ReasonList="<%# PhoenixCrewTravelRequest.ListTravelReason(null) %>" />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Class" HeaderStyle-Width="70px">
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel runat="server" ID="lblClass" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTRAVELCLASSNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel runat="server" ID="lblClass" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTRAVELCLASS") %>'></telerik:RadLabel>
                                        <eluc:Hard ID="ucClassOld" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                            HardList="<%# PhoenixRegistersHard.ListHard(1,227) %>" HardTypeCode="227" SelectedHard='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELCLASS") %>' />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Hard ID="ucClassAdd" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                            HardList="<%# PhoenixRegistersHard.ListHard(1,227) %>" HardTypeCode="227" SelectedHard='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELCLASS") %>' />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width="50px" HeaderText="Action">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Edit"
                                            CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                            ToolTip="Edit" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" AlternateText="Delete"
                                            CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                            ToolTip="Delete" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save"
                                            CommandName="UPDATE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                            ToolTip="Save Break Journey" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" AlternateText="Cancel"
                                            CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                            ToolTip="Cancel" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                        </asp:LinkButton>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Add"
                                            CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                            ToolTip="Add New" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                        </asp:LinkButton>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </telerik:GridTableView>
                    </DetailTables>

                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Name/NOK of Employee" HeaderStyle-Width="120px">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDNAME") %>'
                                    CommandName="SELECT"></asp:LinkButton>
                                /
                                    <br />
                                <asp:LinkButton ID="lnkFName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFNAME") %>'
                                    CommandName="SELECT"></asp:LinkButton>
                                <telerik:RadLabel ID="lblEmployeeId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmpId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblTravelRequestId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblOfficeTravel" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICEREQUESTYN") %>'></telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblTravelReqNo" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUISITIONNO") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDOB" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDDATEOFBIRTH", "{0:dd/MM/yyyy}")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblppno" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPASSPORTNO")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblothervisadet" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDOTHERVISADETAILS")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-Width="78px">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="true" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblVesselName" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="On/Off-Signer" HeaderStyle-Width="98px">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="true" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDONSIGNERYESNO") %>
                                <telerik:RadLabel runat="server" ID="lblonoffsignerid" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNERYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" HeaderStyle-Width="70px">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="90px"></HeaderStyle>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDRANKNAME")%>
                                <telerik:RadLabel runat="server" ID="lblRankId" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblRankIdEdit" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'></telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblRankName" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblOfficeTravelYN" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICEREQUESTYN") %>'></telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblFamilyTravelYN" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYTRAVELYN") %>'></telerik:RadLabel>
                                <eluc:Rank ID="ddlRankEdit" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" Width="80px"
                                    RankList="<%#PhoenixRegistersRank.ListRank() %>" SelectedRank='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Payment Mode" HeaderStyle-Width="120px">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPaymentmode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPAYMENTMODENAME")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Hard ID="ucPaymentmode" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="80px"
                                    HardList="<%# PhoenixRegistersHard.ListHard(1,185) %>" HardTypeCode="185" SelectedHard='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTMODE") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Origin" HeaderStyle-Width="130px">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDORIGINNAME")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListAirportEdit">
                                    <telerik:RadLabel ID="lbloriginname" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDORIGINNAME")%>'></telerik:RadLabel>
                                    <telerik:RadTextBox ID="txtAirportNameEdit" runat="server" Width="75%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGINNAME") %>'
                                        Enabled="False" CssClass="input_mandatory">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowAirportEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" ImageAlign="Top" Text=".." CommandName="BUDGETCODE"
                                        OnClientClick="return showPickList('spnPickListAirportEdit', 'codehelp1', '', '../Common/CommonPickListCity.aspx', true); " />
                                    <telerik:RadTextBox ID="txtoriginIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGINID") %>'
                                        Width="0px" CssClass="hidden">
                                    </telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Destination" HeaderStyle-Width="130px">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDDESTINATIONNAME")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblDesname" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDDESTINATIONNAME")%>'></telerik:RadLabel>
                                <span id="spnPickListAirportdestinationedit">
                                    <telerik:RadTextBox ID="txtDestinsationNameedit" runat="server" Width="75%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONNAME") %>'
                                        Enabled="False" CssClass="input_mandatory">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowDestinationedit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="Top" Text=".." CommandName="BUDGETCODE"
                                        OnClientClick="return showPickList('spnPickListAirportdestinationedit', 'codehelp1', '', '../Common/CommonPickListCity.aspx', true); " />
                                    <telerik:RadTextBox ID="txtDestinationIdedit" runat="server" Width="0px" CssClass="hidden"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONID") %>'>
                                    </telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Departure" HeaderStyle-Width="190px">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDepartureDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDTRAVELDATE", "{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbldepartureampm" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREAMPM") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblDepdate" runat="server" Visible="false" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDTRAVELDATE", "{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbldepartureampmedit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREAMPM") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTravelRequestIdedit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <eluc:Date runat="server" ID="txtDepartureDate" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTRAVELDATE", "{0:dd/MM/yyyy}")%>'
                                    Width="110px"></eluc:Date>
                                <telerik:RadComboBox ID="ddlampmdeparture" runat="server" CssClass="dropdown_mandatory" Width="45px"
                                    Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True" EmptyMessage="Type to select">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="AM" Value="1" />
                                        <telerik:RadComboBoxItem Text="PM" Value="2" />
                                    </Items>
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Arrival" HeaderStyle-Width="190px">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblArrivalDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE", "{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblarrivalampm" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDARRIVALAMPM") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblArrdate" runat="server" Visible="false" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE", "{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblarrivalampmedit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDARRIVALAMPM") %>'></telerik:RadLabel>
                                <eluc:Date runat="server" ID="txtArrivalDate" CssClass="input" Text='<%# DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE")%>'
                                    Width="112px"></eluc:Date>
                                <telerik:RadComboBox ID="ddlampmarrival" runat="server" CssClass="input" Width="45px"
                                    EnableLoadOnDemand="True" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="AM" Value="1" />
                                        <telerik:RadComboBoxItem Text="PM" Value="2" />
                                    </Items>
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="74px">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbltravelstatus" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDISCANCELLEDYNSTATUS")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ToolTip="Edit" Width="20PX" Height="20PX"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" ToolTip="Delete" Width="20PX" Height="20PX"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel Travel" ToolTip="Cancel Travel" Width="20PX" Height="20PX"
                                    CommandName="CANCELTRAVEL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancelTravel">
                                <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                                <eluc:CommonToolTip ID="ucCommonToolTip" runat="server" Screen='<%# "Crew/CrewTravelMoreInfoList.aspx?empId=" + DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID").ToString() +"&familyId=" + DataBinder.Eval(Container,"DataItem.FLDFAMILYID").ToString()%>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                    ToolTip="Save" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="false" SaveScrollPosition="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br />
            <telerik:RadLabel ID="lblAgent" runat="server" Font-Bold="true" Text="Assigned Agent"></telerik:RadLabel>

            <eluc:TabStrip ID="MenuAgentList" runat="server" OnTabStripCommand="MenuAgentList_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvAgent" runat="server" AllowCustomPaging="true" AllowSorting="true" RetainExpandStateOnRebind="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvAgent_ItemCommand" OnNeedDataSource="gvAgent_NeedDataSource" AllowPaging="true"
                OnItemDataBound="gvAgent_ItemDataBound" EnableViewState="true" GroupingEnabled="false" EnableHeaderContextMenu="true" OnDetailTableDataBind="gvAgent_DetailTableDataBind">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EnableHierarchyExpandAll="true" DataKeyNames="FLDTRAVELAGENTID,FLDAGENTID" AllowMultiColumnSorting="True" AutoGenerateColumns="false">
                    <DetailTables>
                        <telerik:GridTableView EnableHierarchyExpandAll="true" AutoGenerateColumns="false" Name="gvAgentQuote" runat="server" AllowPaging="false" DataKeyNames="FLDQUOTEID,FLDAGENTID">
                            <DetailTables>
                                <telerik:GridTableView Name="gvAgentQuoteDetails" Width="100%" DataKeyNames="FLDQUOTEID" AllowPaging="false" AutoGenerateColumns="false">
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="Name">
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblrequestid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblquoteid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEID") %>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblagentid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGENTID") %>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblisapproved" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISAPPROVED") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Origin">
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblorg" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGINNAME") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Destination">
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lbldest" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONNAME") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Fare">
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTAMOUNT") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Tax">
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblTax" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTTAX") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Total Amount">
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblTotalAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTTOTALAMOUNT") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Total Amount(USD)(Incl. Fare + Tax)">
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblTotalAmountusd" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALAMOUNT") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Travel Status">
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblTravelstatusHeader" runat="server">Travel Status</telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lbltravelstatus" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDISCANCELLEDYNSTATUS")%>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblcancellyn" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDISCANCELLEDYN")%>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Quotation Number">
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblQuotationNumberHeader" runat="server">Quotation No</telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblQuotationNumber" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDQUOTATIONREFNO") +" "+ "-" + " "+DataBinder.Eval(Container, "DataItem.FLDAGENTCODE")%>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblSelectedQuoteId" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDSELECTEDQUOTEID") %>'
                                                    Visible="false">
                                                </telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Action">
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" AlternateText="Approve" ID="cmdApprove" CommandName="PASSENGERAPPROVE"
                                                    ToolTip="Approve" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-award"></i></span>
                                                </asp:LinkButton>
                                                <asp:LinkButton runat="server" AlternateText="Revoke approval" ID="cmdDeApprove" CommandName="PASSENGERDEAPPROVE"
                                                    ToolTip="Revoke approval" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-redo"></i></span>
                                                </asp:LinkButton>
                                                <asp:LinkButton runat="server" AlternateText="Send Mail" ID="cmdSemdMail" CommandName="PASSENGERSENDMAIL"
                                                    ToolTip="Send Mail" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-envelope"></i></span>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </telerik:GridTableView>
                                <telerik:GridTableView Name="gvLineItem" Width="100%" DataKeyNames="FLDQUOTEID" AllowPaging="false" AutoGenerateColumns="false">
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="Name">
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblEmployeeId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                                    Visible="false">
                                                </telerik:RadLabel>
                                                <telerik:RadLabel ID="lblRouteID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROUTEID") %>'
                                                    Visible="false">
                                                </telerik:RadLabel>
                                                <telerik:RadLabel ID="lblQuotationID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEID") %>'
                                                    Visible="false">
                                                </telerik:RadLabel>
                                                <telerik:RadLabel ID="lblTravelID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELID") %>'
                                                    Visible="false">
                                                </telerik:RadLabel>
                                                <telerik:RadLabel ID="lblRequestID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                                    Visible="false">
                                                </telerik:RadLabel>
                                                <telerik:RadLabel ID="lblBreakupID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBREAKUPID") %>'
                                                    Visible="false">
                                                </telerik:RadLabel>
                                                <telerik:RadLabel ID="lblattachmentyn" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTACHMENTYN") %>'
                                                    Visible="false">
                                                </telerik:RadLabel>
                                                <telerik:RadLabel ID="lblDtKey" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'
                                                    Visible="false">
                                                </telerik:RadLabel>
                                                <telerik:RadLabel ID="lnkName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDNAME") %>' CommandName="SELECT">
                                                </telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Origin">
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblorg" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGIN") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Destination">
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lbldest" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATION") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Departure">
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblDepartureDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDEPARTUREDATE", "{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                                                <%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREAMPM") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Arrival">
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblArrivalDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE", "{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                                                <%# DataBinder.Eval(Container,"DataItem.FLDARRIVALAMPM") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Stops">
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnknostop" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOOFSTOPS") %>'></asp:LinkButton>
                                                  <telerik:RadLabel ID="lblAmount" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Duration">
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblDuration" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDURATION") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Airline Code">
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblairlinecode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRLINECODE") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Class">
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblTravelClass" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELCLASSNAME") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Action">
                                            <ItemStyle Wrap="false" HorizontalAlign="Center" />
                                            <ItemTemplate>                                         
                                                <asp:LinkButton runat="server" AlternateText="Itinerary" ToolTip="Itinerary" Width="20PX" Height="20PX"
                                                    CommandArgument="<%# Container.DataSetIndex %>" ID="cmdShowReason">
                               <span class="icon"><i class="fas fa-list-alt-Itinerary"></i></span>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </telerik:GridTableView>
                            </DetailTables>
                            <Columns>
                                <telerik:GridTemplateColumn HeaderStyle-Width="102px" HeaderText="Quotation No.">
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblQuotationID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEID") %>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="lblTravelID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELID") %>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="lblRequestID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="lblAgentID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGENTID") %>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="lblIsApproved" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.APPOVEDSTATUS") %>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="lblIsFinalized" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINALIZEDYN") %>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="lbltravelfinalizeyn" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELFINALIZEYN") %>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="lblQuotationNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTATIONREFNO") %>'>
                                        </telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width="102px" HeaderText="Quotation Date">
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblQuotationDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDRECEIVEDDATE", "{0:dd/MMM/yyyy}")) +" "+ DataBinder.Eval(Container, "DataItem.FLDRECEIVEDDATE", "{0:t}")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width="60px" HeaderText="Currency">
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width="62px" HeaderText="Fare">
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTAMOUNT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width="62px" HeaderText="Tax" AllowSorting="true" SortExpression="FLDTAX" ShowSortIcon="true">
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTax" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTTAX") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width="85px" HeaderText="Total">
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTotalAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTTOTALAMOUNT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width="85px" HeaderText=" Total(USD)">
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTotalAmountusd" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALAMOUNT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width="60px" HeaderText="Quoted Status">
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTATIONSTATUS") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width="85px" HeaderText="Fare Type">
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblmarinefare" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISMARINEFAREYN") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width="85px" HeaderText="Approved Amount(USD)">
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblApprovedAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVEDTOTALUSDAMOUNT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width="85px" HeaderText="Approved By">
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblApproveBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.APPROVERNAME") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblApproveDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDTRAVELAPPROVEDDATE", "{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width="85px" HeaderText="PO Date">
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblPODate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDPOSENTDATE", "{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width="100px" HeaderText="Action">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Approve" ID="cmdApprove" CommandName="APPROVE"
                                            ToolTip="Approve" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-award"></i></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" AlternateText="Finalize" ID="cmdFinalize" CommandName="FINALIZE"
                                            ToolTip="Finalize" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-check-circle-fq"></i></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" AlternateText="Email" ID="cmdSemdMail" CommandName="SENDMAIL"
                                            CommandArgument='<%# Container.DataSetIndex %>' ToolTip="Send PO" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-envelope"></i></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" AlternateText="View Ticket" ToolTip="View Ticket" Visible="false"
                                            CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAttachmentMapping">
                               <span class="icon"><i class="fas fa-paperclip"></i></span>
                                        </asp:LinkButton>
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
                        </telerik:GridTableView>
                    </DetailTables>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="4%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblImageHeader" runat="server"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="chkSelect" Checked="true" runat="server" EnableViewState="true" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Agent">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTravelID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblTravelAgentId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELAGENTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblAgentID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGENTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <asp:LinkButton ID="lnkAgentName" runat="server" CommandName="SELECT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGENTNAME") + " - " + DataBinder.Eval(Container,"DataItem.FLDAGENTCODE")%> '></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sent Date">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSendDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDSENDDATE", "{0:dd/MMM/yyyy}")) +" "+ DataBinder.Eval(Container, "DataItem.FLDSENDDATE", "{0:t}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Received Date">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRecievedDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDRECEIVEDDATE", "{0:dd/MMM/yyyy}"))  +" "+ DataBinder.Eval(Container, "DataItem.FLDRECEIVEDDATE", "{0:t}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete"
                                    CommandArgument="<%# Container.DataSetIndex %>" ToolTip="Delete" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Communication" CommandName="COMMUNICATION" ID="cmdCommunication"
                                    CommandArgument="<%# Container.DataSetIndex %>" ToolTip="Chat" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-comments"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Quote" CommandName="NEWQUOTE" ID="cmdNewQuote"
                                    CommandArgument="<%# Container.DataSetIndex %>" ToolTip="New Quote" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="3" EnableColumnClientFreeze="true"
                        ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
