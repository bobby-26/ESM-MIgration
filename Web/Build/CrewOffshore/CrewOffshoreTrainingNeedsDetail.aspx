<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreTrainingNeedsDetail.aspx.cs" Inherits="CrewOffshoreTrainingNeedsDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToBeDoneBy" Src="~/UserControls/UserControlOffshoreToBeDoneBy.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Recommended Courses</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>


</head>
<body>
    <form id="frmCrewRecommendedCourses" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <div class="navigation" id="Div1" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">


                <eluc:TabStrip ID="TrainingNeed" runat="server" TabStrip="true" OnTabStripCommand="TrainingNeed_TabStripCommand"></eluc:TabStrip>

                <table width="100%">
                    <tr align="left">
                        <td>
                            <telerik:RadLabel ID="lblfileno" runat="server" Text="File No."></telerik:RadLabel>

                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtfileno" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblfirstname" runat="server" Text="Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="lblfname" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblRANKname" runat="server" Text="Rank"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="lblrname" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>


                    </tr>


                </table>
                <div>
                    <telerik:RadRadioButtonList ID="rbtoption" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rbtoption_SelectedIndexChanged" Direction="Horizontal">
                        <Items>
                            <telerik:ButtonListItem Text="Pending" Value="PENDING"></telerik:ButtonListItem>
                            <telerik:ButtonListItem Text="Completed" Value="COMPLETED"></telerik:ButtonListItem>
                            <telerik:ButtonListItem Text="Overriden" Value="OVERRIDEN"></telerik:ButtonListItem>
                        </Items>

                    </telerik:RadRadioButtonList>
                </div>

                <telerik:RadLabel ID="lbloverride" runat="server" Text="Pending Training Course"
                    Font-Bold="true">
                </telerik:RadLabel>

                <eluc:TabStrip ID="MenuOffshoreTraining" runat="server" OnTabStripCommand="MenuOffshoreTraining_TabStripCommand"></eluc:TabStrip>

                <div id="divGrid" style="position: relative; z-index: 1">
                    <%-- <asp:GridView ID="gvOffshoreTraining" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowFooter="false" OnRowCommand="gvOffshoreTraining_RowCommand" OnRowCreated="gvOffshoreTraining_RowCreated"
                        OnRowEditing="gvOffshoreTraining_RowEditing" OnRowCancelingEdit="gvOffshoreTraining_RowCancelingEdit"
                        OnRowUpdating="gvOffshoreTraining_RowUpdating" ShowHeader="true" EnableViewState="false" DataKeyNames="FLDTRAININGNEEDID"
                        OnRowDataBound="gvOffshoreTraining_RowDataBound" OnRowDeleting="gvOffshoreTraining_OnRowDeleting">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvOffshoreTraining" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvOffshoreTraining_NeedDataSource"
                        OnItemCommand="gvOffshoreTraining_ItemCommand"
                        OnItemDataBound="gvOffshoreTraining_ItemDataBound"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top" DataKeyNames="FLDTRAININGNEEDID">
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
                            <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                            <Columns>

                                <telerik:GridTemplateColumn UniqueName="Status1" HeaderText="Status">
                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                    <HeaderStyle Width="50px" />
                                    <ItemTemplate>
                                        <asp:Image ID="ImgFlagP" runat="server" alt="" ImageUrl="<%$ PhoenixTheme:images/Red.png %>"
                                            Visible="false" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn Visible="false" UniqueName="Name2" HeaderText="Name">
                                    <HeaderStyle Width="150px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblEmployeeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblRankId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblExamID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblCourseRequestId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSEREQUESTID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblExamRequestId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMREQUESTID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblTrainingNeedId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGNEEDID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblIsRaisedFromCBT" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISRAISEDFROMCBT") %>'></telerik:RadLabel>
                                        <asp:LinkButton ID="lnkEployeeName" runat="server" CommandArgument="<%#Container.DataSetIndex%>"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME")%>' CommandName="GETEMPLOYEE"></asp:LinkButton>
                                        <telerik:RadLabel ID="lblName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Rank3" Visible="false">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblAppliedRankHeader" runat="server" Text="Rank"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="fileno4" Visible="false">
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblFileNoHeader" runat="server" Text="File No"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Vessel5" HeaderText="Vessel">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblVessel" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Category6" HeaderText="Category">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="125px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCATEGORYNAME").ToString().Length > 40 ? DataBinder.Eval(Container, "DataItem.FLDCATEGORYNAME").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDCATEGORYNAME").ToString() %>'></telerik:RadLabel>
                                        <eluc:ToolTip ID="ucCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblIsRaisedFromCBTEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISRAISEDFROMCBT") %>'></telerik:RadLabel>
                                        <telerik:RadComboBox Width="100%" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select" ID="ddlCategoryEdit" Enabled="false" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                            AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCategoryEdit_SelectedIndexChanged">
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadComboBox Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select" ID="ddlCategoryAdd" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                            AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCategoryAdd_SelectedIndexChanged">
                                        </telerik:RadComboBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="SubCategory7" HeaderText="Sub Category">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="125px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSubCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSUBCATEGORYNAME").ToString().Length > 40 ? DataBinder.Eval(Container, "DataItem.FLDSUBCATEGORYNAME").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDSUBCATEGORYNAME").ToString() %>'></telerik:RadLabel>
                                        <eluc:ToolTip ID="ucSubCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYNAME") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadComboBox Width="100%" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select" ID="ddlSubCategoryEdit" Enabled="false" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true">
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadComboBox Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select" ID="ddlSubCategoryAdd" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true">
                                        </telerik:RadComboBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="IDN8" HeaderText="Identified Training Need">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="125px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTrainingNeed" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGNEED") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox Width="100%" ID="txtTrainingNeedEdit" Enabled="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGNEED") %>'
                                            CssClass="gridinput_mandatory">
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadTextBox ID="txtTrainingNeedAdd" runat="server" CssClass="gridinput_mandatory"></telerik:RadTextBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="LM9" HeaderText="Level of Improvement">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblLevelOfImprovement" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELOFIMPROVEMENTNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Quick ID="ucImprovementEdit" Width="100%" Enabled="false" runat="server" CssClass="input_mandatory" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELOFIMPROVEMENT") %>'
                                            AppendDataBoundItems="true" QuickTypeCode="128" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Quick ID="ucImprovementAdd" runat="server" CssClass="input_mandatory"
                                            AppendDataBoundItems="true" QuickTypeCode="128" />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="TT10" Visible="false">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblTypeofTrainingHeader" runat="server" Text="Type of Training"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTypeofTraining" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFTRAININGNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Quick ID="ucTypeofTrainingEdit" Width="100%" runat="server" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFTRAINING") %>'
                                            AppendDataBoundItems="true" QuickTypeCode="129" Enabled="false" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="course11" HeaderText="Course/ CBT">
                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                    <HeaderStyle Width="65px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCourseName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREQUESTEDCOURSES").ToString() %>'></telerik:RadLabel>
                                        <asp:LinkButton ID="imgCourseName" runat="server" CommandArgument='<%# Container.DataSetIndex %>'>
                                            <span class="icon"><i class="fas fa-glasses"></i></span>
                                        </asp:LinkButton>
                                        <eluc:ToolTip ID="ucToolTipCourseName" Width="450px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREQUESTEDCOURSES") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Material12" HeaderText="Material">
                                    <HeaderStyle Width="65px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Material"
                                            CommandName="MATERIAL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdMaterial"
                                            ToolTip="Download Material">
                                             <span class="icon"><i class="fas fa-download"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="tobedone13" HeaderText="Due Date">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDoneby1" runat="server" Visible="true" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDUEDATE"))  %>'></telerik:RadLabel>

                                        <%--<telerik:RadLabel ID="lblDoneby" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOBEDONEBYNAME") %>'></telerik:RadLabel>--%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:ToBeDoneBy ID="ucDonebyEdit" Width="100%" runat="server" AppendDataBoundItems="true" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="TI14" HeaderText="Details of Training Imparted / Course Attended">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDetails" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILSOFTRAINING") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtDetailsEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILSOFTRAINING") %>'>
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="NTI15" HeaderText="Name of Trainer / Institute">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTrainer" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFTRAINERINSTITUTE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtTrainerEdit" Width="100%" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFTRAINERINSTITUTE") %>'>
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="DC16" HeaderText="Date Completed">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCompletionDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATECOMPLETED")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Date ID="ucCompletionDate" runat="server" Width="100%" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATECOMPLETED")) %>' />
                                        <telerik:RadLabel ID="lblDateCompleted" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATECOMPLETED")) %>' Visible="false"></telerik:RadLabel>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="test17" HeaderText="Test">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="60px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblExam" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="attempt18" HeaderText="No. Of Attempts">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderStyle Width="60px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblNoOfAttempts" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOOFATTEMPTS") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="score19" HeaderText="Score %">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderStyle Width="60px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCORE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="result20" HeaderText="Result">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="60px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblResult" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXAMRESULT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="DA21" HeaderText="Date Attended">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblDateAttendedHeader" runat="server" Text="Date Attended"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDateAttended" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATEATTENDED")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="DAR22" HeaderText="Date Archived" Visible="false">
                                    <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="75px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDateArchived" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATEARCHIVED")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="override23" HeaderText="Override Y/N" Visible="false">
                                    <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkOverrideYN" runat="server" Checked='<%# DataBinder.Eval(Container,"DataItem.FLDOVERRIDEYN").ToString().Equals("1")? true : false %>' Enabled="false" />
                                        <telerik:RadLabel ID="lblOverrideYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERRIDEYN") %>' Visible="false"></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="action" HeaderAbbr="Action">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>

                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Edit"
                                            CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                            ToolTip="Edit">
                                             <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Attachment"
                                            CommandName="ATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAtt"
                                            ToolTip="Upload Attachment">
                                            <span class="icon"><i class="fas fa-paperclip"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Initiate Course Request"
                                            CommandName="Initiate Course Request" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCourseReq"
                                            ToolTip="View Requested Course" Visible="false">
                                            <span class="icon"><i class="fas fa-book"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Initiate Test Request"
                                            CommandName="INITIATEEXAMREQ" CommandArgument="<%# Container.DataSetIndex %>"
                                            ID="cmdExamReq" ToolTip="Test">
                                            <span class="icon"><i class="fas fa-pen-alt"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Initiate Course Request"
                                            CommandName="Test Request History" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdExamReqHistory"
                                            ToolTip="Test History">
                                             <span class="icon"><i class="fas fa-pen-alt"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Archive"
                                            CommandName="Archive" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdArchive" Visible="false"
                                            ToolTip="Archive">
                                            <span class="icon"><i class="fas fa-history"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="De-Archive"
                                            CommandName="DeArchive" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDeArchive" Visible="false"
                                            ToolTip="De-Archive">
                                              <span class="icon"><i class="fas fa-archive"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Override"
                                            CommandName="OVERRIDE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdOverride"
                                            ToolTip="Training Need Override">
                                              <span class="icon"><i class="fas fa-calendar-times"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save"
                                            CommandName="UPDATE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                            ToolTip="Save">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Cancel"
                                            CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                            ToolTip="Cancel">
                                            <span class="icon"><i class="fas fa-times-circle"></i></span>
                                        </asp:LinkButton>
                                    </EditItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                    <FooterTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save"
                                            CommandName="Add" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAdd"
                                            ToolTip="Add New">
                                            <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                        </asp:LinkButton>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>

                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="300px" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </div>
                <table cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <img id="lblRed" runat="server" src="<%$ PhoenixTheme:images/red-symbol.png %>" />
                        </td>
                        <td>
                            <asp:Literal ID="lblOverDue" runat="server" Text=" * Overdue"></asp:Literal>
                        </td>
                        <td>
                            <img id="lblGreen" runat="server" src="<%$ PhoenixTheme:images/green-symbol.png %>" />
                        </td>
                        <td>
                            <asp:Literal ID="lblDue" runat="server" Text=" * Due"></asp:Literal>
                        </td>
                    </tr>
                </table>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
