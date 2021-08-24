<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportPreSeaCoursesDoneFormat2.aspx.cs" Inherits="CrewReportPreSeaCoursesDoneFormat2" %>

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
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvCrewPreSeaCoursesDone.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
        <style type="text/css">
            .mlabel {
                color: blue !important;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPreSeaCoursesDone" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskinmanager1" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <span style="float: left">
                <eluc:Title runat="server" ID="ucTitle" Text="Courses Done" ShowMenu="true" />
                <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            </span>

            <eluc:TabStrip ID="MenuReports" runat="server" OnTabStripCommand="MenuReports_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>

            <eluc:TabStrip ID="MenuPreSeaCourseDoneReport" runat="server" OnTabStripCommand="MenuPreSeaCourseDoneReport_TabStripCommand"></eluc:TabStrip>

            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>File Number
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFileNo" runat="server" Width="400px"></telerik:RadTextBox>
                    </td>
                    <td>Status
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlStatus" runat="server" DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE"
                            EmptyMessage="Type to select status" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="300px">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>Rank
                    </td>
                    <td>
                        <eluc:UserControlRankList ID="ucRank" runat="server" Width="400px" AppendDataBoundItems="true" />
                    </td>
                    <td>Batch
                    </td>
                    <td>
                        <eluc:Batch ID="ucBatch" runat="server" AppendDataBoundItems="true" Width="300px" />
                    </td>
                </tr>
                <tr>
                    <td>Course
                    </td>
                    <td>
                        <telerik:RadListBox ID="lstCourse" runat="server" AppendDataBoundItems="true" Width="400px"
                            SelectionMode="Multiple" Height="60px">
                        </telerik:RadListBox>
                    </td>
                    <td>Institution
                    </td>
                    <td>

                        <telerik:RadComboBox ID="lstInstitution" runat="server" DataTextField="FLDNAME" DataValueField="FLDADDRESSCODE"
                            EmptyMessage="Type to select institution" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="300px">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>Include new applicant
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkIncludeNewApp" runat="server" />
                    </td>
                    <td>Institute Status
                    </td>
                    <td>
                        <eluc:Hard ID="ucStatus" runat="server" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>Vessel
                    </td>
                    <td>
                        <eluc:UserControlVesselList ID="ucVessel" runat="server" Width="400px" Height="80px"
                            AppendDataBoundItems="true" Entitytype="VSL" AssignedVessels="true" VesselsOnly="true" />
                    </td>

                    <td>Pool
                    </td>
                    <td>
                        <eluc:UserControlPool ID="ucPool" runat="server" AppendDataBoundItems="true"
                            Width="300px" />
                    </td>
                </tr>
                <tr>

                    <td>Manager
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ucManager" runat="server" DataTextField="FLDNAME" DataValueField="FLDADDRESSCODE"
                            EmptyMessage="Type to select Manager" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="300px">
                        </telerik:RadComboBox>
                    </td>
                    <td>Issued Date
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td>From
                                </td>
                                <td>
                                    <eluc:Date ID="txtFromDate" runat="server" />
                                </td>
                                <td>To
                                </td>
                                <td>
                                    <eluc:Date ID="txtToDate" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>

            </table>

            <eluc:TabStrip ID="MenuCrewCoursesDone" runat="server" OnTabStripCommand="CrewCoursesDone_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid ID="gvCrewPreSeaCoursesDone" runat="server" AutoGenerateColumns="False" Font-Size="11px" OnSorting="gvCrewPreSeaCoursesDone_Sorting"
                CellPadding="3" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="true" OnItemCommand="gvCrewPreSeaCoursesDone_ItemCommand" OnSelectedIndexChanging="gvCrewPreSeaCoursesDone_SelectedIndexChanging"
                OnItemDataBound="gvCrewPreSeaCoursesDone_ItemDataBound" CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvCrewPreSeaCoursesDone_NeedDataSource" RenderMode="Lightweight" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                    AllowNaturalSort="false" AutoGenerateColumns="false" TableLayout="Fixed">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ForeColor="Black" ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>

                        <telerik:GridButtonColumn Text="DoubleClick" CommandName="SELECT" Visible="false" />
                        <telerik:GridTemplateColumn HeaderText="S No." HeaderStyle-Width="90px">

                            <ItemTemplate>
                                <telerik:RadLabel ForeColor="Black" ID="lblSrNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROW") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Emp Code" HeaderStyle-Width="80px">

                            <ItemTemplate>
                                <telerik:RadLabel ForeColor="Black" ID="lblFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="150px">

                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEmployeeName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="true" SortExpression="FLDRANK" HeaderStyle-Width="60px">

                            <ItemTemplate>
                                <telerik:RadLabel ForeColor="Black" ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Qualification" HeaderStyle-Width="90px">
                            <ItemTemplate>
                                <eluc:ToolTip ID="ucToolTipQualification" runat="server" Width="100%" Position="Center" TargetControlId="lblQualification" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPLANATION") %>' />
                                <telerik:RadLabel ForeColor="Black" ID="lblQualification" runat="server" CssClass="tooltip" ClientIDMode="AutoID" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUALIFICATION").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDQUALIFICATION").ToString().Substring(0, 10)+ "..." : DataBinder.Eval(Container, "DataItem.FLDQUALIFICATION").ToString() %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Institution" HeaderStyle-Width="90px">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInstitution" runat="server" ClientIDMode="AutoID" ForeColor="Black" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSTITUTIONNAME").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDINSTITUTIONNAME").ToString().Substring(0, 10)+ "..." : DataBinder.Eval(Container, "DataItem.FLDINSTITUTIONNAME").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipInstitution" runat="server" TargetControlId="lblInstitution" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSTITUTIONNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Institution Status" HeaderStyle-Width="90px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInstituteStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSTITUTIONSTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Place of Issue" HeaderStyle-Width="90px">

                            <ItemTemplate>
                                <telerik:RadLabel ForeColor="Black" ID="lblPlaceofIssue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFINSTITUTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="From" HeaderStyle-Width="70px">

                            <ItemTemplate>
                                <%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDFROMDATE"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="To" HeaderStyle-Width="70px">

                            <ItemTemplate>
                                <%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDTODATE"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date of Passing" HeaderStyle-Width="90px">

                            <ItemTemplate>
                                <%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDPASSDATE"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="60px">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" ForeColor="Black" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Present Vessel" HeaderStyle-Width="120px">

                            <ItemTemplate>
                                <telerik:RadLabel ForeColor="Black" ID="lblCurrentVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRESENTVESSEL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Last Vessel" HeaderStyle-Width="120px">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExVessel" runat="server" ForeColor="Black" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTVESSEL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="S/off Date" HeaderStyle-Width="90px">

                            <ItemTemplate>
                                <%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDSIGNOFFDATE"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="% in academics" HeaderStyle-Width="90px">

                            <ItemTemplate>
                                <telerik:RadLabel ForeColor="Black" ID="lblacademics" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERCENTAGE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Grade" HeaderStyle-Width="60px">

                            <ItemTemplate>
                                <telerik:RadLabel ForeColor="Black" ID="lblGrade" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGRADE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" HeaderStyle-Width="90px">

                            <ItemTemplate>
                                <telerik:RadLabel ForeColor="Black" ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Batch" HeaderStyle-Width="90px">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBatch" ForeColor="Black" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCH") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="O'all Rank" HeaderStyle-Width="90px">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOAllRank" ForeColor="Black" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOALLRANK") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Onboard" HeaderStyle-Width="90px">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPresentVessel" runat="server" ForeColor="Black" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRESENTVESSEL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="S/on Date" HeaderStyle-Width="70px">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignOnDate" runat="server" ForeColor="Black" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Course Name" HeaderStyle-Width="90px">

                            <ItemTemplate>
                                <telerik:RadLabel ForeColor="Black" ID="lblCourseId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ForeColor="Black" ID="lblEmpid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ForeColor="Black" ID="lblCourseName" runat="server" CssClass="tooltip" ClientIDMode="AutoID" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSENAME").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDCOURSENAME").ToString().Substring(0, 10)+ "..." : DataBinder.Eval(Container, "DataItem.FLDCOURSENAME").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipCourse" runat="server" TargetControlId="lblCourseName" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSENAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Issue Date" AllowSorting="true" SortExpression="FLDDATE" HeaderStyle-Width="70px">

                            <ItemTemplate>
                                <telerik:RadLabel ForeColor="Black" ID="lblIssueDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
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
