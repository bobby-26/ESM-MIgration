<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshorePendingCourseRequest.aspx.cs" Inherits="CrewOffshorePendingCourseRequest" %>

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
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
          <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvReq.ClientID %>"));
                }, 200);
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewAboutUsBy" runat="server">

        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
       
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

            <table cellspacing="1" cellpadding="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="UcVessel" runat="server"  AppendDataBoundItems="true"
                            AutoPostBack="true" OnTextChangedEvent="ucVessel_Changed" Width="150px" />
                    </td>
                    <td colspan="2"></td>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtEmployeeName" ></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <br />

            <eluc:TabStrip ID="CourseRequestMenu" runat="server"
                OnTabStripCommand="CourseRequestMenu_TabStripCommand" />

         
                <%--  <asp:GridView ID="gvReq" runat="server" AllowSorting="true"
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
                <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvReq" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        CellSpacing="0" GridLines="None" OnNeedDataSource="gvReq_NeedDataSource"
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
                                  <HeaderStyle Width="50PX" />
                                    <ItemTemplate>
                                        <%#(Container.DataSetIndex+1)%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Vessel Name">
                                   <HeaderStyle Width="150PX" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblVesselName" runat="server"
                                            Text='<%#DataBinder.Eval(Container, "DataItem.FLDVESSELNAME")%>'>
                                        </telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="File No">
                                    <HeaderStyle Width="100px" />

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Name">
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblEmployeeCode" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDEMPLOYEECODE")%>' Visible="false"></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblEmployeeName" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDEMPLOYEENAME")%>' Visible="false"></telerik:RadLabel>
                                        <asp:LinkButton ID="lnkEmployeeName" CommandName="SELECTROW" CommandArgument='<%# Container.DataSetIndex %>'
                                            runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>' Width="200px"></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Docuemnt Name">
                                    <HeaderStyle Width="150PX" />
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
                                        <telerik:RadLabel ID="lblIsCancelRequestYN" runat="server"
                                            Text='<%# DataBinder.Eval(Container, "DataItem.FLDISCANCELREQUESTYN")%> ' Visible="false">
                                        </telerik:RadLabel>
                                        <%# DataBinder.Eval(Container,"DataItem.FLDCOURSENAME") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Will be Arranged by">
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
                                <telerik:GridTemplateColumn HeaderText="Name of Institute">

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
                                <telerik:GridTemplateColumn HeaderText="Cost">
                                    <ItemStyle HorizontalAlign="Right" Width="100px" Wrap="False" />
                                    <FooterStyle HorizontalAlign="Right" />
                              
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
                                <telerik:GridTemplateColumn HeaderText="Airfare Cost">
                                    <ItemStyle HorizontalAlign="Right" Width="100px" Wrap="False" />
                              
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblAirfareCost" runat="server"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRFARECOST") %>'>
                                        </telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txt" runat="server" BorderStyle="None" Enabled="false"
                                            Height="0px" Width="0px"></telerik:RadTextBox>
                                        <eluc:Number ID="txtAirfareCost" runat="server" 
                                            Text='<%# DataBinder.Eval(Container, "DataItem.FLDAIRFARECOST") %>'
                                            Width="80px" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Hotel Cost">
                                    <ItemStyle HorizontalAlign="Right" Width="100px" Wrap="False" />
                              
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
                                <telerik:GridTemplateColumn HeaderText="Agency and Other Cost">
                                    <ItemStyle HorizontalAlign="Right" Width="100px" Wrap="False" />
                                
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
                                <telerik:GridTemplateColumn HeaderText="Action">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                 
                                    <ItemStyle HorizontalAlign="Center" Width="100px" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="cmdEdit" runat="server" AlternateText="EDIT"
                                            CommandArgument="<%# Container.DataSetIndex %>" CommandName="EDIT"
                                             ToolTip="Edit" >
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>
                                    
                                        <asp:LinkButton runat="server" AlternateText="Delete" 
                                            CommandName="UNPLAN" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                            ToolTip="Cancel Request">
                                            <span class="icon"><i class="fa fa-trash"></i></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" AlternateText="Complete" 
                                            CommandName="COMPLETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdComplete"
                                            ToolTip="Complete Course Request">
                                            <span class="icon"><i class="fas fa-check-square"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="cmdSave" runat="server" AlternateText="Save"
                                            CommandArgument="<%# Container.DataSetIndex %>" CommandName="Update"
                                            ToolTip="Save" >
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
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true"  ScrollHeight="415px"  />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </telerik:RadAjaxPanel>

                <eluc:Status ID="ucStatus" runat="server" />
                <eluc:Confirm ID="ucConfirm" runat="server" CancelText="Cancel" OKText="Ok"
                    OnConfirmMesage="InitiateCourseRequest" Visible="false" />
       
    </form>
</body>
</html>
