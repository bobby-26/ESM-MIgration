<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreCourseMissing.aspx.cs"
    Inherits="CrewOffshoreCourseMissing" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="../UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="../UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MUCCity" Src="~/UserControls/UserControlMultiColumnCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TravelReason" Src="~/UserControls/UserControlTravelReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Documents" Src="../UserControls/UserControlDocuments.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationality.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Course Details</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewAboutUsBy" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>


            <eluc:TabStrip ID="CrewMenu" runat="server" OnTabStripCommand="CrewMenu_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>

            <table cellspacing="1" cellpadding="1" width="100%">
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
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRankName" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRank1" runat="server" Text=" Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlRank" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                            DataTextField="FLDRANKNAME" DataValueField="FLDRANKID" AutoPostBack="true" Width="150px" OnTextChanged="ddlRank_Changed">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMatrix" runat="server" Text="Training Matrix"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID='lblTraningMatrixID' runat="server" Visible="false" Text=''></telerik:RadLabel>
                        <telerik:RadComboBox ID="ddlTrainingMatrix" runat="server" Width="300px" CssClass="input_mandatory"
                            AppendDataBoundItems="true" AutoPostBack="true">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselID" runat="server" Visible="false"></telerik:RadLabel>
                        <eluc:Vessel runat="server" ID="ucVessel"  AppendDataBoundItems="true"
                            VesselsOnly="true" AssignedVessels="true" AutoPostBack="true" OnTextChangedEvent="ucVessel_Changed" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblExpectedJoiningDate" runat="server" Text="Expected Joining Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtExpectedJoiningDate" runat="server" CssClass="input_mandatory"
                            OnTextChangedEvent="txtExpectedJoiningDate_Changed" AutoPostBack="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselType" Visible="false" runat="server" Text="Vessel Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:VesselType ID="ucVesselType" runat="server" Visible="false" AppendDataBoundItems="true" CssClass="readonlytextbox"
                            Enabled="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOffsigner" runat="server" Text="Off-signer" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlOffSigner" runat="server" Width="242px"  Visible="false"
                            AppendDataBoundItems="true" />
                    </td>
                </tr>
            </table>
            <br />
            <b>
                <telerik:RadLabel ID="ltrlMissingCourse" runat="server" Text="Missing/Expired Course"></telerik:RadLabel>
            </b>
            <br />
            <telerik:RadLabel ID="lblNote" runat="server" CssClass="guideline_text">
                Note: Please note validity of document checked for "contract +3 months" starting from the expected joining date.
            </telerik:RadLabel>

           

            <div id="div1" style="position: relative; z-index: 3; width: 100%;">
                <%--  <asp:GridView ID="gvMissingCourse" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" ShowHeader="true" OnRowDataBound="gvMissingCourse_RowDataBound"
                    OnRowCommand="gvMissingCourse_RowCommand" OnRowCancelingEdit="gvMissingCourse_RowCancelingEdit"
                    OnRowUpdating="gvMissingCourse_RowUpdating" OnRowEditing="gvMissingCourse_RowEditing"
                    EnableViewState="false" AllowSorting="true" ShowFooter="false">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />--%>

                <telerik:RadGrid RenderMode="Lightweight" ID="gvMissingCourse" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                    CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvMissingCourse_NeedDataSource"
                    OnItemDataBound="gvMissingCourse_ItemDataBound"
                    OnItemCommand="gvMissingCourse_ItemCommand"
                    AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top">
                        <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
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
                            <telerik:GridTemplateColumn HeaderText="S.No">
                                <ItemTemplate>
                                    <%#(Container.DataSetIndex+1)%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <HeaderTemplate>
                                    Name
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblEmployeeID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEMPLOYEEID")%>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblCourseId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOURSEID")%>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblRankId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDRANKID")%>'></telerik:RadLabel>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDCOURSENAME") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="From Date">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblFromDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDFROMDATE"))%>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="txtFromDate" runat="server" CssClass="input_mandatory" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDFROMDATE"))%>' />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="To Date">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblToDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDTODATE"))%>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="txtToDate" runat="server" CssClass="input_mandatory" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDTODATE"))%>' />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                <FooterStyle HorizontalAlign="Right" />
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblStatusHeader" runat="server">
                                        Status
                                    </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblAction" runat="server" Text="Action"></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="EDIT"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                        ToolTip="Edit">
                                     <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>

                                    <asp:LinkButton runat="server" AlternateText="Request"
                                        CommandName="REQUEST" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdRequest"
                                        ToolTip="Initiate Request">
                                    <span class="icon"><i class="fas fa-file-import"></i></span>
                                    </asp:LinkButton>

                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Save"
                                        CommandName="Update" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                        ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                    </asp:LinkButton>

                                    <asp:LinkButton runat="server" AlternateText="Cancel"
                                        CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel">
                                     <span class="icon"><i class="fa fa-trash"></i></span>
                                    </asp:LinkButton>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="" EnableNextPrevFrozenColumns="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>

            </div>
            <br />
            <b>
                <telerik:RadLabel ID="lblText2" runat="server" Text="Course Request"></telerik:RadLabel>
            </b>
            <br />

           

                <%-- <asp:GridView ID="gvReq" runat="server" AllowSorting="true"
                    AutoGenerateColumns="False" CellPadding="3" EnableViewState="false"
                    Font-Size="11px" OnRowCancelingEdit="gvReq_RowCancelingEdit"
                    OnRowCommand="gvReq_RowCommand" OnRowCreated="gvReq_RowCreated"
                    OnRowDataBound="gvReq_RowDataBound" OnRowDeleting="gvReq_RowDeleting"
                    OnRowEditing="gvReq_RowEditing" OnRowUpdating="gvReq_RowUpdating"
                    OnSorting="gvReq_Sorting" ShowFooter="false" ShowHeader="true" Width="100%">
                    <FooterStyle CssClass="datagrid_footerstyle" />
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvReq" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                    CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvReq_NeedDataSource"
                    OnItemCommand="gvReq_ItemCommand"
                    OnItemDataBound="gvReq_ItemDataBound"
                    AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top">
                        <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
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
                            <telerik:GridTemplateColumn HeaderText="S.No">
                                <HeaderStyle Width="50px" />
                                <ItemTemplate>
                                    <%#(Container.DataSetIndex+1)%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <HeaderStyle Width="150px" />
                                <HeaderTemplate>
                                    Name
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRefNo" runat="server"
                                        Text='<%# DataBinder.Eval(Container, "DataItem.FLDREFNO")%> ' Visible="false">
                                    </telerik:RadLabel>
                                    <telerik:RadLabel ID="lblRequestID" runat="server"
                                        Text='<%# DataBinder.Eval(Container, "DataItem.FLDREQUESTID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <telerik:RadLabel ID="lblCourseId" runat="server"
                                        Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOURSEID")%>' Visible="false">
                                    </telerik:RadLabel>
                                    <telerik:RadLabel ID="lblEmployeeID" runat="server"
                                        Text='<%# DataBinder.Eval(Container, "DataItem.FLDEMPLOYEEID")%>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <telerik:RadLabel ID="lblRankID" runat="server"
                                        Text='<%# DataBinder.Eval(Container, "DataItem.FLDRANKID")%>' Visible="false">
                                    </telerik:RadLabel>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDCOURSENAME") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Will be Arranged by">
                                <HeaderStyle Width="150px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblArrangedById" runat="server"
                                        Text='<%#DataBinder.Eval(Container, "DataItem.FLDARRANGEDBY")%>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <telerik:RadLabel ID="lblArrangedBy" runat="server"
                                        Text='<%#DataBinder.Eval(Container, "DataItem.FLDARRANGEDBYNAME")%>'>
                                    </telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:RadioButtonList ID="rblArrangedBy" runat="server"
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Candidate" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Company" Value="2"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="From Date">
                                <HeaderStyle Width="150px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblFromDate" runat="server"
                                        Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDFROMDATE"))%>'>
                                    </telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>

                                    <telerik:RadLabel ID="lblRequestIDEdit" runat="server"
                                        Text='<%# DataBinder.Eval(Container, "DataItem.FLDREQUESTID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <eluc:Date ID="txtFromDate" runat="server" CssClass="input_mandatory"
                                        DatePicker="true"
                                        Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDFROMDATE"))%>' />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="To Date">
                                <HeaderStyle Width="150px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblToDate" runat="server"
                                        Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDTODATE"))%>'>
                                    </telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="txtToDate" runat="server" CssClass="input_mandatory"
                                        DatePicker="true"
                                        Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDTODATE"))%>' />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <HeaderStyle Width="200px" />
                                <HeaderTemplate>
                                    Name of Institute
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblInstitue" runat="server"
                                        Text='<%#DataBinder.Eval(Container, "DataItem.FLDINSTITUTENAME")%>'>
                                    </telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Address ID="ucInstitution" runat="server"
                                        AddressList='<%# PhoenixRegistersAddress.ListAddress("138") %>'
                                        AddressType="138" AppendDataBoundItems="true"  Width="300px" />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemStyle HorizontalAlign="Right" Width="100px" Wrap="False" />
                                <FooterStyle HorizontalAlign="Right" />
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblPlaceHeader" runat="server" Text="Place">
                                    </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPlace" runat="server"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACE") %>'>
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemStyle HorizontalAlign="Right" Width="100px" Wrap="False" />
                                <FooterStyle HorizontalAlign="Right" />
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblCostHeader" runat="server">
                                        Cost
                                    </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCost" runat="server"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOST") %>'>
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Wages Payable">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblWagesPayable" runat="server" Text="Wages Payable">
                                    </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblWagesPayable" runat="server"
                                        Text='<%# (DataBinder.Eval(Container,"DataItem.FLDWAGESPAYABLE"))%>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <telerik:RadLabel ID="lblWagesPay" runat="server"
                                        Text='<%# (DataBinder.Eval(Container,"DataItem.FLDWAGESPAYABLE").ToString().Equals("1"))?"Yes":"No" %>'>
                                    </telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkWagesPayable" runat="server"
                                        Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDWAGESPAYABLE").ToString().Equals("1"))?true:false %>' />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemStyle HorizontalAlign="Right" Width="100px" Wrap="False" />
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblAirfareCostHeader" runat="server" Text="Airfare Cost">
                                    </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblAirfareCost" runat="server"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRFARECOST") %>'>
                                    </telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox ID="txt" runat="server" BorderStyle="None" Enabled="false"
                                        Height="0px" Width="0px">
                                    </telerik:RadTextBox>
                                    <eluc:Number ID="txtAirfareCost" runat="server" 
                                        Text='<%# DataBinder.Eval(Container, "DataItem.FLDAIRFARECOST") %>'
                                        Width="80px" />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemStyle HorizontalAlign="Right" Width="100px" Wrap="False" />
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblHotelCostHeader" runat="server" Text="Hotel Cost">
                                    </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblHotelCost" runat="server"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDHOTELCOST") %>'>
                                    </telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="txtHotelCost" runat="server" 
                                        Text='<%# DataBinder.Eval(Container, "DataItem.FLDHOTELCOST") %>'
                                        Width="80px" />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemStyle HorizontalAlign="Right" Width="100px" Wrap="False" />
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblOtherCostHeader" runat="server" Text="Agency and Other Cost">
                                    </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblOtherCost" runat="server"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHERCOST") %>'>
                                    </telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="txtOtherCost" runat="server" 
                                        Text='<%# DataBinder.Eval(Container, "DataItem.FLDOTHERCOST") %>'
                                        Width="80px" />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblAction" runat="server" Text="Action"></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="100px" Wrap="False" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="cmdEdit" runat="server" AlternateText="EDIT"
                                        CommandArgument="<%# Container.DataSetIndex %>" CommandName="EDIT"
                                        ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>

                                    <asp:LinkButton runat="server" AlternateText="Delete"
                                        CommandName="UNPLAN" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                        ToolTip="Cancel Request">
                                    <span class="icon"><i class="fas fa-calendar-times"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="cmdSave" runat="server" AlternateText="Save"
                                        CommandArgument="<%# Container.DataSetIndex %>" CommandName="Update"
                                        ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                    </asp:LinkButton>
                                    <img id="Img1" runat="server" alt=""
                                        src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:LinkButton ID="cmdCancel" runat="server" AlternateText="Cancel"
                                        CommandArgument="<%# Container.DataSetIndex %>" CommandName="Cancel"
                                        ToolTip="Cancel">
                                    <span class="icon"><i class="fa fa-trash"></i></span>
                                    </asp:LinkButton>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="1" ScrollHeight=""   EnableNextPrevFrozenColumns="true"/>
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
                <eluc:Status ID="ucStatus" runat="server" />
                <eluc:Confirm ID="ucConfirm" runat="server" CancelText="Cancel" OKText="Ok"
                    OnConfirmMesage="InitiateCourseRequest" Visible="false" />
         
            <br />
            <br />
            <b>
                <telerik:RadLabel ID="lblCourses" runat="server" Text="Course Completion"></telerik:RadLabel>
            </b>
         
                <%-- <asp:GridView ID="gvCrewCourseCertificate" runat="server" AutoGenerateColumns="False"
                    Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvCrewCourseCertificate_RowCommand"
                    OnRowDataBound="gvCrewCourseCertificate_RowDataBound" OnRowDeleting="gvCrewCourseCertificate_RowDeleting"
                    OnRowCancelingEdit="gvCrewCourseCertificate_RowCancelingEdit" OnSelectedIndexChanging="gvCrewCourseCertificate_SelectedIndexChanging"
                    OnRowEditing="gvCrewCourseCertificate_RowEditing" OnRowUpdating="gvCrewCourseCertificate_RowUpdating"
                    ShowFooter="false" ShowHeader="true" EnableViewState="false">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="10px" />--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewCourseCertificate" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                    CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvCrewCourseCertificate_NeedDataSource"
                  OnItemCommand="gvCrewCourseCertificate_ItemCommand"
                    OnItemDataBound="gvCrewCourseCertificate_ItemDataBound"
                    AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top">
                        <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
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
                            <telerik:GridButtonColumn Text="DoubleClick" CommandName="SELECT" Visible="false" />
                            <telerik:GridTemplateColumn HeaderText="S.No">
                                <HeaderStyle Width="50px" />
                                <itemtemplate>
                                <telerik:RadLabel ID="lblCertificateSNO" Text='<%#(Container.DataSetIndex+1)%>' runat="server"></telerik:RadLabel>
                            </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                 <HeaderStyle Width="50px" />
                                <itemtemplate>
                                <asp:Image ID="imgFlag" runat="server" Visible="false" />
                            </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                 <HeaderStyle Width="100px" />
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <headertemplate>
                                <telerik:RadLabel runat="server" ID="lblCourseTypeHeader">Type&nbsp;</telerik:RadLabel>
                            </headertemplate>
                                <itemtemplate>
                                <telerik:RadLabel ID="lblCourseType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                            </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <HeaderStyle Width="100px" />
                                <headertemplate>
                                <telerik:RadLabel ID="lblCourseHeader" runat="server">Course &nbsp;                                        
                                </telerik:RadLabel>
                            </headertemplate>
                                <itemtemplate>
                                <asp:LinkButton ID="lnkDoubleClick" runat="server" Visible="false" CommandName="Edit"
                                    CommandArgument='<%# Container.DataSetIndex %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblCourseId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEECOURSEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbldocid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCourseName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSENAME") %>'></telerik:RadLabel>
                            </itemtemplate>
                                <edititemtemplate>
                                <asp:LinkButton ID="lnkDoubleClickEdit" runat="server" Visible="false" CommandName="Edit"
                                    CommandArgument='<%# Container.DataSetIndex %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblCourseId" runat="server"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOURSEID")%>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCourseIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEECOURSEID") %>'></telerik:RadLabel>
                                <eluc:Course ID="ddlCourseEdit" runat="server" CourseList="<%# PhoenixRegistersDocumentCourse.ListNonCBTDocumentCourse() %>"
                                    ListCBTCourse="false" SelectedCourse='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSE") %>'
                                    AppendDataBoundItems="true" CssClass="dropdown_mandatory" AutoPostBack="true" Visible="false"
                                    OnTextChangedEvent="ddlDocument_TextChanged" />
                                <telerik:RadLabel ID="lbldocid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSENAME") %>'></telerik:RadLabel>
                            </edititemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn FooterText="Number">
                                <HeaderStyle Width="100px" />
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <headertemplate>
                                <telerik:RadLabel ID="lblCourseNumberHeader" runat="server">Certificate Number&nbsp;
                                </telerik:RadLabel>
                            </headertemplate>
                                <itemtemplate>
                                <asp:LinkButton ID="lnkCourseNumber" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSENUMBER") %>'></asp:LinkButton>
                            </itemtemplate>
                                <edititemtemplate>
                                <telerik:RadTextBox ID="txtCourseNumberEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSENUMBER") %>'
                                    CssClass="gridinput_mandatory" MaxLength="30">
                                </telerik:RadTextBox>
                            </edititemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <HeaderStyle Width="100px" />
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <headertemplate>
                                <telerik:RadLabel ID="lblPlace" Text='Place of Issue' runat="server"></telerik:RadLabel>
                            </headertemplate>
                                <itemtemplate>
                                <telerik:RadLabel ID="lblPlaceOfIssue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE") %>'></telerik:RadLabel>
                            </itemtemplate>
                                <edititemtemplate>
                                <telerik:RadTextBox ID="txtPlaceOfIssueEdit" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE")%>'>
                                </telerik:RadTextBox>
                            </edititemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Issue Date">
                                <HeaderStyle Width="150px" />
                                <headertemplate>
                                <telerik:RadLabel runat="server" ID="lblIssueDateHeader" Text="Issue Date"></telerik:RadLabel>
                            </headertemplate>
                                <itemtemplate>
                                <telerik:RadLabel ID="lblIssueDate" runat="server" Visible="true" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE")) %>'></telerik:RadLabel>
                            </itemtemplate>
                                <edititemtemplate>
                                <eluc:Date runat="server" ID="txtIssueDateEdit" CssClass="input_mandatory" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE")) %>' />
                            </edititemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <HeaderStyle Width="150px" />
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <headertemplate>
                                <telerik:RadLabel ID="lblExpiry" Text='Expiry Date' runat="server"></telerik:RadLabel>
                            </headertemplate>
                                <itemtemplate>
                                <telerik:RadLabel ID="lblExpiryDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY")) %>'></telerik:RadLabel>
                            </itemtemplate>
                                <edititemtemplate>
                                <eluc:Date runat="server" ID="txtExpiryDateEdit" CssClass='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRY").ToString() == "1" ? ("input_mandatory") : ("input") %>'
                                    Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY")) %>' />
                            </edititemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <HeaderStyle Width="150px" />
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <headertemplate>
                                <telerik:RadLabel ID="lblInstitution" Text='Institution' runat="server"></telerik:RadLabel>
                            </headertemplate>
                                <itemtemplate>
                                <telerik:RadLabel ID="lblInstitution" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSTITUTENAME") %>'></telerik:RadLabel>
                            </itemtemplate>
                                <%-- <EditItemTemplate>
                                    <eluc:Address runat="server" ID="ucInstitutionEdit" CssClass="input_mandatory" AppendDataBoundItems="true"
                                        AddressType="138" AddressList='<%# PhoenixRegistersAddress.ListAddress("138") %>'
                                        SelectedAddress='<%# DataBinder.Eval(Container,"DataItem.FLDINSTITUTIONID") %>' />
                                </EditItemTemplate>       --%>                        
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Nationality">
                                <HeaderStyle Width="100px" />
                                <itemtemplate>
                                <telerik:RadLabel ID="lblNationality" Text='<%# DataBinder.Eval(Container, "DataItem.FLDNATIONALITY").ToString()%>' runat="server"></telerik:RadLabel>
                            </itemtemplate>
                                <edititemtemplate>
                                <eluc:Nationality ID="ddlFlagEdit" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                    NationalityList='<%#PhoenixRegistersCountry.ListNationality() %>' SelectedNationality='<%# DataBinder.Eval(Container, "DataItem.FLDCOUNTRYCODE").ToString()%>' />
                            </edititemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Issuing Authority">
                                <HeaderStyle Width="100px" />
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <itemtemplate>
                                <telerik:RadLabel ID="lblAuthority" Text='<%# DataBinder.Eval(Container, "DataItem.FLDAUTHORITY")%>' runat="server"></telerik:RadLabel>
                            </itemtemplate>
                                <edititemtemplate>
                                <telerik:RadTextBox ID="txtAuthorityEdit" runat="server"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDAUTHORITY")%>'
                                    MaxLength="100">
                                </telerik:RadTextBox>
                            </edititemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <headerstyle horizontalalign="Center" verticalalign="Middle"></headerstyle>
                                <headertemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    <telerik:RadLabel ID="lblCourseAction" Text="Action" runat="server"></telerik:RadLabel>
                                </telerik:RadLabel>
                            </headertemplate>
                                <itemstyle wrap="False" horizontalalign="Center" width="100px"></itemstyle>
                                <itemtemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" 
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                
                                <asp:LinkButton runat="server" AlternateText="Delete" 
                                    CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                    ToolTip="Delete" Visible="false">
                                    <span class="icon"><i class="fa fa-trash"></i></span>
                                </asp:LinkButton>
                            </itemtemplate>
                                <edititemtemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" 
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                    ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                
                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel">
                                    <span class="icon"><i class="fa fa-trash"></i></span>
                                </asp:LinkButton>
                            </edititemtemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" >
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="1" ScrollHeight=""  EnableNextPrevFrozenColumns="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
          
        </div>

    </form>
</body>
</html>
