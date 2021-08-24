<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCourseMissing.aspx.cs"
    Inherits="CrewCourseMissing" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="../UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="../UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MUCCity" Src="~/UserControls/UserControlMultiColumnCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TravelReason" Src="~/UserControls/UserControlTravelReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head" runat="server">
    <title>Initiate Course Request</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
         <style type="text/css">
           .fon {
            font-size: small !important;
        }
        </style>
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
    <form id="frmCrewAboutUsBy" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
            <asp:Button ID="confirm" runat="server" Text="confirm" OnClick="confirm_Click" />

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="CrewMenu" runat="server" OnTabStripCommand="CrewMenu_TabStripCommand"></eluc:TabStrip>

            <table cellspacing="1" cellpadding="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ucVessel" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                            AutoPostBack="true" Entitytype="VSL" ActiveVesselsOnly="true" VesselsOnly="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRankName" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        <span id="Span1" class="icon" runat="server" visible="false"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                        <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip1" Width="400px" ShowEvent="onmouseover" CssClass="fon"
                            RelativeTo="Element" Animation="Fade" TargetControlID="Span1" IsClientID="true"
                            HideEvent="ManualClose" Position="MiddleRight" EnableRoundedCorners="true" ContentScrolling="Auto" VisibleOnPageLoad="true"
                            Text="Note:Course request won't be initiated until rank is updated.">
                        </telerik:RadToolTip>
                    </td>
                </tr>
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
                        <telerik:RadLabel ID="lblCourseType" runat="server" Text="Course Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ucDocumentType" runat="server" AppendDataBoundItems="true" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select course type"
                            AutoPostBack="true" DataValueField="FLDHARDCODE" DataTextField="FLDHARDNAME" OnTextChanged="ucDocumentType_TextChanged">
                            <Items>
                                <telerik:RadComboBoxItem Value="Dummy" Text="--Select--" />
                            </Items>
                        </telerik:RadComboBox>
                        <%-- <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>--%>
                        
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIncludeCourseDone" runat="server" Text="Include courses done"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkIncludeCoursesDone" runat="server" AutoPostBack="true" />
                    </td>
                </tr>
            </table>
            <hr />
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtFromDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtToDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAccommodationRequired" runat="server" Text="Accommodation Required"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkAccom" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox runat="server" ID="txtRemarks" CssClass="input_mandatory" TextMode="MultiLine"
                            Width="350px" Height="50px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="lblTravelRequiredYN" Visible="false" runat="server" Text="Travel Required"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkTravel" runat="server" AutoPostBack="true" OnCheckedChanged="OnClickTravelRequest"
                            Visible="false" />
                    </td>
                </tr>
            </table>
            <hr />
            <div id="dvTravel" runat="server">
                <b>
                    <telerik:RadLabel ID="lblTravelDetails" runat="server" Text="Travel Details"></telerik:RadLabel>
                </b>
                <br />
                <table width="80%" cellpadding="2" cellspacing="2">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblTravelDate" runat="server" Text="Travel Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucTravelDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblArrivalDate" runat="server" Text="Arrival Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucArrivalDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblOrigin" runat="server" Text="Origin"></telerik:RadLabel>
                        </td>
                        <td colspan="5">
                            <eluc:MUCCity ID="ucOrigin" runat="server" CssClass="input_mandatory" />
                        </td>
                        <td colspan="5">
                            <telerik:RadLabel ID="lblTravelReason" runat="server" Text="Travel Reason"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:TravelReason ID="ucTravelReason" runat="server" AppendDataBoundItems="true"
                                CssClass="dropdown_mandatory" ReasonFor="1" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblDestination" runat="server" Text="Destination"></telerik:RadLabel>
                        </td>
                        <td colspan="5">
                            <eluc:MUCCity ID="ucDestination" runat="server" CssClass="input_mandatory" />
                        </td>
                        <td colspan="5">
                            <telerik:RadLabel ID="lblUnallocatedVessel" runat="server" Text="Unallocated Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadCheckBox ID="chkUnallocatedVessel" runat="server" />
                        </td>
                    </tr>
                </table>
                <br />
            </div>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvMissingCourse" runat="server"  AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvMissingCourse_NeedDataSource" GroupingEnabled="false" EnableHeaderContextMenu="true"               
                AutoGenerateColumns="false" EnableViewState="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
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
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Select">
                            <HeaderStyle Width="25px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="25px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="chkSelect" runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type">
                            <HeaderStyle Width="50px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDDOCUMENTTYPE")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Code">
                            <HeaderStyle Width="50px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDABBREVIATION")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Course">
                            <HeaderStyle Width="150px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDocumentId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDDOCUMENTID")%>'></telerik:RadLabel>
                                <%#DataBinder.Eval(Container, "DataItem.FLDCOURSE")%>
                            </ItemTemplate>
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
            <br />
            <br />

            <eluc:TabStrip ID="CourseRequestMenu" runat="server" OnTabStripCommand="CourseRequestMenu_TabStripCommand"></eluc:TabStrip>
            <div id="divGrid1" style="position: relative; z-index: 0; width: 100%;">
                <telerik:RadGrid RenderMode="Lightweight" ID="gvReq" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="gvReq_NeedDataSource"
                    OnSortCommand="gvReq_SortCommand"
                    OnItemDataBound="gvReq_ItemDataBound"
                    GroupingEnabled="false" EnableHeaderContextMenu="true"
                    AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
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
                        <HeaderStyle Width="102px" />
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Select">
                                <HeaderStyle Width="50px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="25px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadCheckBox ID="chkSelect" runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Name">
                                <HeaderStyle Width="150px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRefNo" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREFNO")%> '></telerik:RadLabel>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="File No">
                                <HeaderStyle Width="100px" />
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDFILENO")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Accom Req.">
                                <HeaderStyle Width="50px" />
                                <ItemTemplate><%#DataBinder.Eval(Container, "DataItem.FLDACCOMYN")%></ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Rank">
                                <HeaderStyle Width="50px" />
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDRANKCODE")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Zone">
                                <HeaderStyle Width="50px" />
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDZONE")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Vessel">
                                <HeaderStyle Width="150px" />
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Courses to do">
                                <HeaderStyle Width="100px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCourse" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOURSE").ToString()%> '></telerik:RadLabel>
                                    <eluc:ToolTip ID="ucToolTipDate" runat="server" Text='<%# "Created By : " + DataBinder.Eval(Container,"DataItem.FLDCREATEDBY") +"<br/>Created Date :  " + DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE","{0:dd/MMM/yyyy}") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Available From">
                                <HeaderStyle Width="100px" />
                                <ItemTemplate><%#DataBinder.Eval(Container, "DataItem.FLDFROMDATE", "{0:dd/MMM/yyyy}") %></ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Available To">
                                <ItemTemplate><%#DataBinder.Eval(Container, "DataItem.FLDTODATE", "{0:dd/MMM/yyyy}") %></ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Remarks">
                                <HeaderStyle Width="100px" />
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString().Substring(0, 10) + "..." : DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString() %>'></telerik:RadLabel>
                                    <eluc:ToolTip ID="ucToolTipAddress" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' TargetControlId="lblRemarks" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Travel RefNo">
                                <HeaderStyle Width="100px" />
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDTRAVELNO")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
                <eluc:Status ID="ucStatus" runat="server" />
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
