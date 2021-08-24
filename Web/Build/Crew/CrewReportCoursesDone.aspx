<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportCoursesDone.aspx.cs"
    Inherits="CrewReportCoursesDone" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Batch" Src="~/UserControls/UserControlPreSeaBatchList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlRankList" Src="../UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlVesselList" Src="../UserControls/UserControlVesselCommonCheckBoxList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Institution" Src="~/UserControls/UserControlInstitution.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlPool" Src="~/UserControls/UserControlPoolList.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
       <%-- <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvCrewCoursesDone.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
            </script--%>
        <style type="text/css">
            .mlabel {
                color: blue !important;                
            }
        </style>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmCoursesDone" runat="server" submitdisabledcontrols="true">
        <%--<telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">--%>
         <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

               <%-- <eluc:Title runat="server" ID="ucTitle" Text="Courses Done" ShowMenu="true" />--%>
                <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            
            <eluc:TabStrip ID="MenuReports" runat="server" OnTabStripCommand="MenuReports_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>

            <eluc:TabStrip ID="MenuCourseDoneReport" runat="server" OnTabStripCommand="MenuCourseDoneReport_TabStripCommand"></eluc:TabStrip>

            <div id="divGuidance" runat="server">
                <table id="tblGuidance">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblcourse" runat="server"   ForeColor="Black" CssClass="mlabel">
                                Note: <br />&nbsp 1.When File No is entered course filter is not mandatory.<br />&nbsp 
                                2.Vessel filter is applicable only if the status is selected as Onboard or Onleave.<br />&nbsp 
                                3.When Sign on/off dates are entered course and issue period filter is not mandatory
                            </telerik:RadLabel>
                        </td>
                    </tr>
                </table>
            </div>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <asp:Panel ID="Issued" runat="server" GroupingText="Issued Date">
                            <table>
                                <tr>
                                    <td>
                                        <telerik:RadLabel   ForeColor="Black" ID="lblIssuedFrom" Text="From" runat="server"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtFromDate" runat="server" />
                                    </td>
                                    <td>
                                        <telerik:RadLabel   ForeColor="Black" ID="lblIssuedTo" Text="To" runat="server"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtToDate" runat="server" CssClass="input_mandatory" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                    <td>
                        <telerik:RadLabel   ForeColor="Black" ID="lblBatch" runat="server" Text="Batch"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Batch ID="ucBatch" runat="server" AppendDataBoundItems="true"  Width="300px" />
                    </td>
                    <td>
                        <telerik:RadLabel  ForeColor="Black"  Text="Institution" ID="lblInstitution" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Address ID="ucInstitution" runat="server"  Width="300px" AppendDataBoundItems="true"
                            AutoPostBack="true" AddressType="138" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlSignon" runat="server" GroupingText="SignOn Date">
                            <table>
                                <tr>
                                    <td>
                                        <telerik:RadLabel  ForeColor="Black"  ID="lblSignonFrom" Text="From" runat="server"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Date ID="ucSignonFromDate" runat="server" />
                                    </td>
                                    <td>
                                        <telerik:RadLabel   ForeColor="Black" ID="lvlSignonTo" Text="To" runat="server"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Date ID="ucSignonToDate" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                    <td>
                        <telerik:RadLabel   ForeColor="Black" Text="Pool" ID="lblPool" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlPool ID="ucPool" runat="server" AppendDataBoundItems="true"  Width="300px" />
                    </td>
                    <td>
                        <telerik:RadLabel   ForeColor="Black" ID="lblManager" runat="server" Text="Manager"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Address ID="ucManager" runat="server" AddressType="126" AppendDataBoundItems="true"
                            Width="300px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlSignoff" runat="server" GroupingText="SignOff Date">
                            <table>
                                <tr>
                                    <td>
                                        <telerik:RadLabel   ForeColor="Black" ID="lblSignoffFrom" Text="From" runat="server"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Date ID="ucSignoffFromDate" runat="server" />
                                    </td>
                                    <td>
                                        <telerik:RadLabel   ForeColor="Black" ID="lblSignoffTo" Text="To" runat="server"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Date ID="ucSignoffToDate" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                    <td>
                        <telerik:RadLabel   ForeColor="Black" ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlVesselList ID="ucVessel" runat="server" AppendDataBoundItems="true"
                          Entitytype="VSL" AssignedVessels="true" VesselsOnly="true"  Width="58%" ActiveVesselsOnly="true"/>
                    </td>
                    <td>
                        <telerik:RadLabel   ForeColor="Black" ID="lblCourseType" runat="server" Text="Course Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard runat="server" ID="ucDocumentType" AppendDataBoundItems="true"
                            HardTypeCode="103" AutoPostBack="true" OnTextChangedEvent="DocumentTypeSelection"
                            ShortNameFilter="1,2,4,5,6" Width="300px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <telerik:RadLabel   ForeColor="Black" ID="lblIncludearchivedcerts" runat="server" Text="Include archived certs"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadCheckBox ID="chkShowArchived" runat="server" />
                                </td>
                                <td>
                                    <telerik:RadLabel  ForeColor="Black"  ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="ddlStatus" runat="server" Width="200px">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="All" Value=" " />
                                            <telerik:RadComboBoxItem Text="OnBoard" Value="1" />
                                            <telerik:RadComboBoxItem Text="OnLeave" Value="0" />
                                            <telerik:RadComboBoxItem Text="Inactive" Value="2" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel   ForeColor="Black" ID="lblIncludenewapplicant" runat="server" Text="Include new applicant"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadCheckBox ID="chkIncludeNewApp" runat="server" />
                                </td>
                                <td>
                                    <telerik:RadLabel   ForeColor="Black" ID="lblFileNumber" runat="server" Text="File No"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtFileNo" runat="server" Width="200px"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel   ForeColor="Black" ID="lblIncludeInactiveSeafarer" runat="server" Text="Include Inactive Seafarer"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadCheckBox ID="chkShowIncative" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <telerik:RadLabel   ForeColor="Black" ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlRankList ID="ucRank" runat="server" AppendDataBoundItems="true"
                            Width="300px" />
                    </td>
                    <td>
                        <telerik:RadLabel   ForeColor="Black" ID="lblCourseFilter" runat="server" Text="Course"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadListBox ID="lstCourse" runat="server" AppendDataBoundItems="true"
                            SelectionMode="Multiple" CssClass="input_mandatory" Height="80px"  Width="300px" />

                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuCrewCoursesDone" runat="server" OnTabStripCommand="CrewCoursesDone_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid ID="gvCrewCoursesDone" runat="server" AutoGenerateColumns="False"  Font-Size="11px" OnSorting="gvCrewCoursesDone_Sorting"
                CellPadding="3" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="false" OnItemCommand="gvCrewCoursesDone_ItemCommand" OnSelectedIndexChanging="gvCrewCoursesDone_SelectedIndexChanging"
                OnItemDataBound="gvCrewCoursesDone_ItemDataBound" CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvCrewCoursesDone_NeedDataSource" RenderMode="Lightweight" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                    AllowNaturalSort="false" AutoGenerateColumns="false" TableLayout="Fixed">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel   ForeColor="Black" ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridButtonColumn Text="DoubleClick" CommandName="SELECT" Visible="false" />
                        <telerik:GridTemplateColumn HeaderText="S No.">

                            <ItemTemplate>
                                <telerik:RadLabel   ForeColor="Black" ID="lblSrNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROW") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Emp Code">

                            <ItemTemplate>
                                <telerik:RadLabel   ForeColor="Black" ID="lblFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name">

                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEmployeeName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="true" SortExpression="FLDRANK">

                            <ItemTemplate>
                                <telerik:RadLabel   ForeColor="Black" ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Zone">

                            <ItemTemplate>
                                <telerik:RadLabel   ForeColor="Black" ID="lblZone" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDZONE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Batch">

                            <ItemTemplate>
                                <telerik:RadLabel   ForeColor="Black" ID="lblBatch" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCH") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status">

                            <ItemTemplate>
                                <telerik:RadLabel  ForeColor="Black"  ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Last Vessel">

                            <ItemTemplate>
                                <telerik:RadLabel   ForeColor="Black" ID="lblExVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTVESSEL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="S/off Date">

                            <ItemTemplate>
                                <telerik:RadLabel  ForeColor="Black"  ID="lblSignOffDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDSIGNOFFDATE", "{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Onboard">

                            <ItemTemplate>
                                <telerik:RadLabel   ForeColor="Black" ID="lblPresentVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRESENTVESSEL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="S/on Date">

                            <ItemTemplate>
                                <telerik:RadLabel   ForeColor="Black" ID="lblSignOnDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDSIGNONDATE", "{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Institution">

                            <ItemTemplate>
                                <telerik:RadLabel   ForeColor="Black" ID="lblInstitution" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSTITUTIONNAME").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDINSTITUTIONNAME").ToString().Substring(0, 10)+ "..." : DataBinder.Eval(Container, "DataItem.FLDINSTITUTIONNAME").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipInstitution" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSTITUTIONNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Place of Issue">

                            <ItemTemplate>
                                <telerik:RadLabel   ForeColor="Black" ID="lblPlaceofIssue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Course Name">

                            <ItemTemplate>
                                <telerik:RadLabel   ForeColor="Black" ID="lblCourseId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel   ForeColor="Black" ID="lblEmpid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel   ForeColor="Black" ID="lblCourseName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSENAME").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDCOURSENAME").ToString().Substring(0, 10)+ "..." : DataBinder.Eval(Container, "DataItem.FLDCOURSENAME").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipCourse" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSENAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issue Date" AllowSorting="true" SortExpression="FLDDATE">

                            <ItemTemplate>
                                <telerik:RadLabel   ForeColor="Black" ID="lblIssueDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATEOFISSUE", "{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expiry Date">

                            <ItemTemplate>
                                <telerik:RadLabel   ForeColor="Black" ID="lblExpiryDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATEOFEXPIRY", "{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
