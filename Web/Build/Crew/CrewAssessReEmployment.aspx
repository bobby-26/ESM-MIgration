<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewAssessReEmployment.aspx.cs"
    Inherits="Crew_CrewAssessReEmployment" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlTooltip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Re-Employment Suitability</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewDateOfAvailability" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>


                <eluc:TabStrip ID="MenuDO" runat="server" Title="Re-Employment suitability" OnTabStripCommand="DOA_TabStripCommand"></eluc:TabStrip>

                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblEmployeeCode" runat="server" Text="Employee Code"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtEmployeeCode" runat="server" CssClass="readonlytextbox" ReadOnly="True"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblEmployeeName" runat="server" Text="Employee Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtEmployeeName" runat="server" MaxLength="50" CssClass="readonlytextbox"
                                ReadOnly="True" Width="150px">
                            </telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblPresentRank" runat="server" Text="Present Rank"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtPayRank" runat="server" MaxLength="20" CssClass="readonlytextbox"
                                ReadOnly="True">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblLastVessel" runat="server" Text="Last Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtLastVessel" runat="server" MaxLength="20" CssClass="readonlytextbox"
                                ReadOnly="True">
                            </telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblSignedOff" runat="server" Text="Signed Off"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadTextBox ID="txtSignedOff" runat="server" MaxLength="20" CssClass="readonlytextbox"
                                ReadOnly="True">
                            </telerik:RadTextBox>
                        </td>

                    </tr>
                </table>
                <br />
                <hr />
                <table width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblAppraisalSatisfactory" runat="server" Text="Are His appraisals from the vessel satisfactory:"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RadioButtonList ID="rblAppraisalSatisfactory" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Not available" Value="3"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblTraining" runat="server" Text="Have his training needs been reviewed and candidate advised of the requirements:"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RadioButtonList ID="rblTraining" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                <asp:ListItem Text="No Training needs" Value="3"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblstblremep" runat="server" Text="Is candidate Suitable for re-employment:"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RadioButtonList ID="rblstblremep" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Requires review by higher authority" Value="2"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>

                <hr />
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblDOAGivenDate" runat="server" Text="DOA Given Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtDOAGivenDate" runat="server" CssClass="input" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblDOAMethod" runat="server" Text="DOA Method"></telerik:RadLabel>
                        </td>
                        <td>

                            <eluc:Quick ID="ddlDOAMethod" runat="server" CssClass="input" AppendDataBoundItems="true"
                                QuickTypeCode="56"></eluc:Quick>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblDateOfTeleconference" runat="server" Text="Date Of Teleconference"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtDTOfTelConf" runat="server" CssClass="input" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblDateOfAvailability" runat="server" Text="Date Of Availability"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtDOA" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblStandbyDate" runat="server" Text="Standby Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtStandbyDate" runat="server" CssClass="input" />
                            <telerik:RadTextBox ID="txtHiddenDOAID" runat="server" MaxLength="6" CssClass="input" Visible="false"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblFollowUpDate" runat="server" Text="FollowUp Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtFollowupDate" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblExpectedsalary" runat="server" Text="Expected Salary"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="txtExpectedsalary" runat="server" CssClass="input txtNumber" MaxLength="10"
                                IsInteger="true" IsPositive="true" />
                        </td>
                    </tr>
                </table>
                <hr />

                <br />
                <b>Training Needs</b>
                <br />

                <%-- <eluc:TabStrip ID="MenuCrewRecommendedCourse" runat="server" OnTabStripCommand="CrewRecommendedCourse_TabStripCommand"></eluc:TabStrip>--%>

                <div id="div2" style="position: relative; z-index: +1">
                    <%-- <asp:GridView ID="gvCrewRecommendedCourses" runat="server" AutoGenerateColumns="False"
                        Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvCrewRecommendedCourses_RowCommand"
                        OnRowDataBound="gvCrewRecommendedCourses_RowDataBound" OnRowCreated="gvCrewRecommendedCourses_RowCreated"
                        ShowFooter="false"
                        ShowHeader="true" EnableViewState="true" AllowSorting="true">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewRecommendedCourses" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvCrewRecommendedCourses_NeedDataSource"
                        OnItemCommand="gvCrewRecommendedCourses_ItemCommand"
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
                            <ColumnGroups>
                                <telerik:GridColumnGroup HeaderText="As reported by the vessel" Name="Office" HeaderStyle-HorizontalAlign="Center">
                                </telerik:GridColumnGroup>
                            </ColumnGroups>
                            <HeaderStyle Width="102px" />
                            <Columns>

                                <telerik:GridTemplateColumn HeaderText="No" ColumnGroupName="Office">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="50px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSno" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROW") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblDocumentType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblWaivedDocumentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAIVEDDOCID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblTrainingNeedID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGNEEDID") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Vessel" ColumnGroupName="Office">
                                    <HeaderStyle Width="150px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblVessel" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Category" ColumnGroupName="Office">
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="130px"></ItemStyle>
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Sub Category" ColumnGroupName="Office">
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="130px"></ItemStyle>
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSubCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Identified Training Need" ColumnGroupName="Office">
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="130px"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTrainingNeed" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGNEED") %>'></telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Level of Improvement" ColumnGroupName="Office">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblLevelOfImprovement" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELOFIMPROVEMENTNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Type of Training">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="40px"></ItemStyle>
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTypeofTraining" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFTRAININGNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="To be done by">
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDoneby" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOBEDONEBYNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="">
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                                    <HeaderStyle Width="85px" />
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkWaivedYN" runat="server" AutoPostBack="true" OnCheckedChanged="chkWaivedYNTN_CheckedChanged"
                                            Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDISWAIVED")).ToString().Equals("2")?true:false %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                            </Columns>
                            <NestedViewTemplate>
                                <table style="font-size: 11px;">
                                    <tr>

                                        <td style="font-weight: 700">
                                            <telerik:RadLabel ID="RadLabel1" runat="server" Text="Waiving Type:"></telerik:RadLabel>

                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblWaivingType" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.FLDWAIVINGTYPENAME")) %>'></telerik:RadLabel>
                                        </td>
                                        <td style="font-weight: 700">
                                            <telerik:RadLabel ID="RadLabel2" runat="server" Text="Reason:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblReason" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.FLDWAIVINGREASON")) %>'></telerik:RadLabel>
                                        </td>
                                        <td style="font-weight: 700">
                                            <telerik:RadLabel ID="RadLabel3" runat="server" Text="Waived Y/N:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblWaiveBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAIVEDUSERNAME") %>'></telerik:RadLabel>
                                        </td>
                                        <td style="font-weight: 700">
                                            <telerik:RadLabel ID="RadLabel5" runat="server" Text="Waived Date:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblWaivedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDWAIVEDDATE")) %>'></telerik:RadLabel>

                                        </td>


                                    </tr>

                                </table>
                            </NestedViewTemplate>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>

                </div>
                <br />
                <b>Appraisal</b>

                <%--  <eluc:TabStrip ID="CrewAppraisal" runat="server" OnTabStripCommand="CrewAppraisal_TabStripCommand"></eluc:TabStrip>--%>

                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <%--    <asp:GridView ID="gvAQ" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowHeader="true" OnRowCommand="gvAQ_RowCommand"
                        OnRowDataBound="gvAQ_RowDataBound"
                        AllowSorting="true" OnSorting="gvAQ_Sorting"
                        EnableViewState="false">
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvAQ" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvAQ_NeedDataSource"
                        OnItemCommand="gvAQ_ItemCommand"
                        OnItemDataBound="gvAQ_ItemDataBound"
                        OnSortCommand="gvAQ_SortCommand"
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
                                <telerik:GridTemplateColumn HeaderText="Vessel Name">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblAppraisalId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCREWAPPRAISALID")%>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblVesselname" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDVESSELNAME")%>'></telerik:RadLabel>
                                        <asp:LinkButton ID="lbtnvesselname" Visible="false" runat="server" CommandArgument='<%# Container.DataSetIndex %>'
                                            Text='<%#DataBinder.Eval(Container, "DataItem.FLDVESSELNAME")%>' CommandName="SELECT"></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="From Date">
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblFromdate" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDFROMDATE", "{0:dd/MMM/yyyy}")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="To Date">
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTodate" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDTODATE", "{0:dd/MMM/yyyy}")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Appraisal Date">
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblAppraisaldate" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAPPRAISALDATE", "{0:dd/MMM/yyyy}")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Occassion For Report">
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblOccassion" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDOCCASSIONFORREPORT")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Total Score">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblScore" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDTOTALSCORE")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Recommended Promotion Y/N">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblPromotion" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROMOTIONYESNO")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Fit for Re-employment Y/N">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblReemployment" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRECOMMENDEDSTATUSNAME")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblActionHeader" runat="server">
                                            Action                                           
                                        </telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Attachment"
                                            CommandName="Attachment" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAtt"
                                            ToolTip="Attachment">
                                            <span class="icon"><i class="fas fa-paperclip"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Appraisal Report"
                                            CommandName="APPRAISAL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAppraisal"
                                            ToolTip="Appraisal Report">
                                            <span class="icon"><i class="fas fa-chart-bar"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Promotion Report"
                                            CommandName="PROMOTION" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdPromotion"
                                            ToolTip="Promotion Report">
                                            <span class="icon"><i class="fas fa-chart-line"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <NestedViewTemplate>
                                <table style="font-size: 11px;">
                                    <tr>

                                        <td style="font-weight: 700">
                                            <telerik:RadLabel ID="RadLabel1" runat="server" Text="HOD Remarks:"></telerik:RadLabel>

                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblHODRemarks" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDHEADDEPTCOMMENT").ToString().Length>20 ? DataBinder.Eval(Container, "DataItem.FLDHEADDEPTCOMMENT").ToString().Substring(0, 20)+ "..." : DataBinder.Eval(Container, "DataItem.FLDHEADDEPTCOMMENT").ToString()%>'></telerik:RadLabel>
                                            <eluc:Tooltip ID="ucHODRemarks" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDHEADDEPTCOMMENT")%>' />
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="font-weight: 700">
                                            <telerik:RadLabel ID="RadLabel2" runat="server" Text="Master Remarks:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblMasterRemarks" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDMASTERCOMMENT").ToString().Length>20 ? DataBinder.Eval(Container, "DataItem.FLDMASTERCOMMENT").ToString().Substring(0, 20)+ "..." : DataBinder.Eval(Container, "DataItem.FLDMASTERCOMMENT").ToString()%>'></telerik:RadLabel>
                                            <eluc:Tooltip ID="ucMasterRemarks" runat="server" Width="200px" Text='<%#DataBinder.Eval(Container, "DataItem.FLDMASTERCOMMENT")%>' />
                                        </td>
                                    </tr>



                                </table>
                            </NestedViewTemplate>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>

                </div>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
