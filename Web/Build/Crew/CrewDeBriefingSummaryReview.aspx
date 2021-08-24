<%@ Page Language="C#" AutoEventWireup="True" CodeFile="CrewDeBriefingSummaryReview.aspx.cs" Inherits="CrewDeBriefingSummaryReview"
    MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonAddress" Src="../UserControls/UserControlCommonAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PhoneNumber" Src="../UserControls/UserControlPhoneNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MobileNumber" Src="../UserControls/UserControlMobileNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToBeDoneBy" Src="~/UserControls/UserControlOffshoreToBeDoneBy.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title>State</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .hidden {
            display: none;
        }

        .center {
            background: fixed !important;
        }
    </style>
</head>

<body>
    <form id="frmRegistersState" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">


                <eluc:TabStrip ID="FeedBackTabs" runat="server" OnTabStripCommand="FeedBackTabs_TabStripCommand"
                    TabStrip="false"></eluc:TabStrip>

                <div id="divPrimarySection" runat="server">
                    <table width="100%" cellpadding="5" cellspacing="1">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="File No"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            </td>

                            <td>
                                <telerik:RadLabel ID="lblFirstName" runat="server" Text="Name"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtFirstName" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                            </td>
                            <td colspan="3">
                                <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkCrewDeBriefingForm" runat="server" Text="Crew De-Briefing Form"></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>


                            <td>
                                <telerik:RadLabel ID="Vessel" runat="server" Text="Vessel"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtvesselName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblSignOnDate" runat="server" Text="Sign On"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtSignOnDate" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                                <%--<eluc:Date ID="txtSignOnDate" runat="server" CssClass="readonly_textbox" ReadOnly="true" />--%>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblSigonOffdate" runat="server" Text="Sign Off"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtSignOffDate" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            </td>


                        </tr>
                        <tr>
                        </tr>
                    </table>
                </div>
                <div>
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <th nowrap>
                                <h3>
                                    <telerik:RadLabel ID="lblavdetail" runat="server" Text="Availability and Contact Details"></telerik:RadLabel>
                                </h3>
                            </th>
                        </tr>
                    </table>
                    <table id="Table1" width="100%" style="color: black; font-size: small; font-family: Arial">
                        <tr>
                            <td>In order that we can plan your next assignment you are requested to kindly advise us your 
                                        <br />
                                availability date and confirm your contact details by filling up the template below.
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table width="20%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lbldoddate" runat="server" Text="Date of Availability"></telerik:RadLabel>
                            </td>
                            <td width="10%">
                                <eluc:Date ID="ucdoa" runat="server" AutoPostBack="true"
                                    OnTextChangedEvent='txtDOA_TextChanged' CssClass="readonlytextbox" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                    <table width="77%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td nowrap>
                                <b>
                                    <telerik:RadLabel ID="lblPermanentAddress" runat="server" Text="Permanent Address"></telerik:RadLabel>
                                </b>
                                &nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp
                            </td>
                            <td colspan="4">
                                <b>
                                    <telerik:RadLabel ID="Literal1" runat="server" Text="Permanent Contact"></telerik:RadLabel>
                                </b>
                            </td>
                        </tr>
                    </table>
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lbladdress1" runat="server" Text="Address1"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtaddress1" Width="75%" CssClass="readonlytextbox" ReadOnly="true" runat="server"></telerik:RadTextBox>
                            </td>
                            <td width="14%">
                                <telerik:RadLabel ID="lblPhoneNumber" runat="server" Text="Phone Number 1"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:PhoneNumber ID="txtPhoneNumber" CssClass="readonlytextbox" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lbladdress2" runat="server" Text="Address2"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtaddress2" Width="75%" CssClass="readonlytextbox" ReadOnly="true" runat="server"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblMobileNumber" runat="server" Text="Mobile Number 1"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:MobileNumber ID="txtMobileNumber" IsMobileNumber="true" runat="server"
                                    CssClass="readonlytextbox" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lbladdress3" runat="server" Text="Address3"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtaddress3" Width="75%" CssClass="readonlytextbox" ReadOnly="true" runat="server"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblEMail" runat="server" Text="E-Mail"></telerik:RadLabel>
                            </td>
                            <td colspan="2">
                                <telerik:RadTextBox ID="txtEmail" runat="server" Width="55%" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lbladdress4" runat="server" Text="Address4"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtaddress4" CssClass="readonlytextbox" ReadOnly="true" Width="75%" runat="server"></telerik:RadTextBox>
                            </td>
                            <td nowrap>
                                <telerik:RadLabel ID="lblPlaceofEngagement" runat="server" Text="Port of Engagement"></telerik:RadLabel>
                            </td>
                            <td colspan="2">
                                <telerik:RadTextBox ID="txtSeaport" runat="server" Width="55%" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCountry" runat="server" Text="Country"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtcountry" CssClass="readonlytextbox" ReadOnly="true" Width="75%" runat="server"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblState" runat="server" Text="State"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtState" CssClass="readonlytextbox" ReadOnly="true" Width="75%" runat="server"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCity" runat="server" Text="City"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtCity" CssClass="readonlytextbox" ReadOnly="true" runat="server" Width="75%"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPostalCode" runat="server" Text="Postal Code"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtPostalCode" CssClass="readonlytextbox" ReadOnly="true" runat="server" Width="75%"></telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <hr />
                <div>
                    <table>
                        <tr>
                            <h3>
                                <telerik:RadLabel ID="lbldebriefinghead" runat="server" Text="De-Briefing:"></telerik:RadLabel>

                            </h3>
                        </tr>
                    </table>

                    <table id="Table2" width="100%" style="color: black; font-size: small; font-family: Arial">
                        <tr>
                            <td>We would also like feedback of your tenure onboard your last vessel and would appreciate if you could kindly fill up the Debriefing template below.
                                        <br />
                                This will allow us to address any issues or concerns that you may have as well as improve our systems
                            </td>
                        </tr>
                    </table>
                    <br />
                </div>
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <%--   <asp:GridView ID="gvFeedBackQst" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowHeader="true"
                        OnPreRender="gvFeedBackQst_PreRender" EnableViewState="false" OnRowDataBound="gvFeedBackQst_RowDataBound">
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvFeedBackQst" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                        CellSpacing="0" GridLines="None" OnNeedDataSource="gvFeedBackQst_NeedDataSource"
                        OnItemDataBound="gvFeedBackQst_ItemDataBound"
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed">
                            <HeaderStyle Width="102px" />
                            <GroupByExpressions>
                                <telerik:GridGroupByExpression>
                                    <SelectFields>
                                        <telerik:GridGroupByField FieldName="FLDCATEGORYNAME" FieldAlias="Question" SortOrder="Ascending" />
                                    </SelectFields>
                                    <GroupByFields>
                                        <telerik:GridGroupByField FieldName="FLDCATEGORYNAME" SortOrder="Ascending" />
                                    </GroupByFields>
                                </telerik:GridGroupByExpression>
                            </GroupByExpressions>
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
                                <telerik:GridTemplateColumn Visible="false" UniqueName="Data" DataField="FLDCATEGORYNAME">
                                    <HeaderStyle Width="20px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblGroup" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDCATEGORYNAME"]%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="99%"></HeaderStyle>
                                    <HeaderTemplate>
                                        Questions
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table cellspacing="10">

                                            <tr>
                                                <td style="font-weight: bold;">
                                                    <telerik:RadLabel ID="lblcategorynameG" Visible="false" Font-Underline="true" runat="server" Font-Size="Small" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCATEGORYNAME")%>'></telerik:RadLabel>
                                                    <br />
                                                    <telerik:RadLabel ID="lblQuestionId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDQUESTIONID")%>'></telerik:RadLabel>
                                                    <%#Container.DataSetIndex+1 %> .
                                                    <telerik:RadLabel ID="lblQuestionName" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDQUESTIONNAME")%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblRequirRemark" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDREQUIREREMARK")%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblCategoryId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCATEGORYID")%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblOrder" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDSORTORDER")%>'
                                                        Visible="false">
                                                    </telerik:RadLabel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:RadioButtonList ID="rblOptions" runat="server" DataValueField="FLDOPTIONID" CssClass="readonlytextbox" ReadOnly="true"
                                                        DataTextField="FLDOPTIONNAME" DataSource='<%# PhoenixCrewDeBriefing.GetdeBriefingoptions(General.GetNullableInteger((DataBinder.Eval(Container,"DataItem.FLDQUESTIONID")).ToString())) %>'
                                                        RepeatDirection="Horizontal">
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trcomments">
                                                <td>
                                                    <telerik:RadLabel ID="lblcomment" Visible="true" runat="server" Text="Comments (If Any)"></telerik:RadLabel>
                                                    <br />
                                                    <telerik:RadTextBox ID="txtComments" CssClass="readonlytextbox" ReadOnly="true" Visible="true" runat="server" TextMode="MultiLine"
                                                        onkeyDown="checkTextAreaMaxLength(this,event,'500');" Width="300px" Height="50px">
                                                    </telerik:RadTextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="false" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>


                    <div>
                        <%--<hr />--%>
                        <h3>
                            <telerik:RadLabel ID="lblGeneralComments" runat="server" Visible="false" Text="General Comments:"></telerik:RadLabel>
                        </h3>
                        <telerik:RadTextBox ID="txtGeneralComments" CssClass="readonlytextbox" Visible="false" runat="server" TextMode="MultiLine"
                            onkeyDown="checkTextAreaMaxLength(this,event,'500');" Width="800px" Height="50px">
                        </telerik:RadTextBox>
                    </div>
                    <div>
                        <hr />
                        <h3>
                            <telerik:RadLabel ID="lblPendingTrainingneeds" runat="server" Text="Recommended Training Needs:"></telerik:RadLabel>
                        </h3>
                    </div>
                    <div id="div2" style="position: relative; z-index: 1">
                        <table id="Table8" width="100%" style="color: black; font-size: small; font-family: Arial">
                            <tr>
                                <td>The following Courses have been identified for you.You are requested to kindly get in touch with your 
                                        <br />
                                    designated manning office so that these can addressed promptly.
                                </td>
                            </tr>
                        </table>
                        <br />

                        <%--<asp:GridView ID="gvCrewRecommendedCourses" runat="server" AutoGenerateColumns="False"
                                Font-Size="11px" Width="100%" CellPadding="3"
                                ShowFooter="false"
                                ShowHeader="true" EnableViewState="false" AllowSorting="true">
                                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                <RowStyle Height="10px" />--%>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewRecommendedCourses" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                            CellSpacing="0" GridLines="None" OnNeedDataSource="gvCrewRecommendedCourses_NeedDataSource"
                            OnItemCommand="gvCrewRecommendedCourses_ItemCommand"
                            GroupingEnabled="false" EnableHeaderContextMenu="true"
                            AutoGenerateColumns="false">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" TableLayout="Fixed">
                                <HeaderStyle Width="102px" />
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

                                    <telerik:GridTemplateColumn HeaderText="Sl.No">
                                        <HeaderStyle Width="30px" />
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container, "DataItem.FLDROWNUMBER")%>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Course">
                                        <ItemStyle HorizontalAlign="Left" Width="20%" Wrap="true"></ItemStyle>
                                        <HeaderStyle Width="150px" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCourseName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Recommended Date">
                                        <ItemStyle HorizontalAlign="Left" Width="5%" Wrap="true"></ItemStyle>
                                        <HeaderStyle Width="100px" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblDtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblrecommendedid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECOMMENDEDID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblEmpid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblRankid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblCourseId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSEID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblRecommendedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECOMMENDEDDATE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Recommended By">
                                        <ItemStyle HorizontalAlign="Left" Width="13%" Wrap="true"></ItemStyle>
                                        <HeaderStyle Width="100px" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblRecommendedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECOMMENDEDBYNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn>
                                        <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="true"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblLastdoneDateHeader" runat="server">Last done Date</telerik:RadLabel>

                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container,"DataItem.FLDCOURSELASTDONE") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Status">
                                        <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="true"></ItemStyle>
                                        <HeaderStyle Width="100px" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDYN") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Approved">
                                        <ItemStyle HorizontalAlign="Left" Width="3%" Wrap="true"></ItemStyle>
                                        <HeaderStyle Width="100px" />
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container, "DataItem.FLDAPPROVEDYN")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadComboBox Width="100%" ID="ddlApproveEdit" runat="server" CssClass="input" OnTextChanged="ddlApprovalType_TextChanged" AutoPostBack="true">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="Dummy" Text="--Select--" />
                                                    <telerik:RadComboBoxItem Value="1" Text="To be done" />
                                                    <telerik:RadComboBoxItem Value="0" Text="Not reqd" />
                                                    <telerik:RadComboBoxItem Value="2" Text="To be done prior next s/on" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Approved Date">
                                        <ItemStyle HorizontalAlign="Left" Width="5%" Wrap="true"></ItemStyle>
                                        <HeaderStyle Width="100px" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblNominatedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOMINATEDDATE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn>
                                        <ItemStyle HorizontalAlign="Left" Wrap="true"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblRemarksHeader" runat="server">Remarks</telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container, "DataItem.FLDAPPROVALREMARKS")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadLabel ID="lblDtkeyEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                            <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="input" Width="100%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVALREMARKS") %>' />
                                        </EditItemTemplate>
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

                        <br />
                        <%-- <asp:GridView ID="gvOffshorePCourseTraining" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" ShowFooter="false" OnRowCommand="gvOffshorePCourseTraining_RowCommand"
                            OnRowCreated="gvOffshorePCourseTraining_RowCreated"
                            ShowHeader="true" EnableViewState="false">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvOffshorePCourseTraining" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                            CellSpacing="0" GridLines="None" OnNeedDataSource="gvOffshorePCourseTraining_NeedDataSource"
                            OnItemCommand="gvOffshorePCourseTraining_ItemCommand"
                            GroupingEnabled="false" EnableHeaderContextMenu="true"
                            AutoGenerateColumns="false">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" TableLayout="Fixed">
                                <HeaderStyle Width="102px" />
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

                                    <telerik:GridTemplateColumn>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblVesselHeader" runat="server" Text="Vessel"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblIsRaisedFromCBT" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISRAISEDFROMCBT") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblVessel" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblCategoryHeader" runat="server" Text="Category"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>

                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblSubCategoryHeader" runat="server" Text="Sub Category"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblSubCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>

                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblTraningHeader" runat="server" Text="Identified Training Need"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblTrainingNeed" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGNEED") %>'></telerik:RadLabel>
                                        </ItemTemplate>

                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblImprovementHeader" runat="server" Text="Level of Improvement"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblLevelOfImprovement" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELOFIMPROVEMENTNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>

                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblTypeofTrainingHeader" runat="server" Text="Type of Training"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblTypeofTraining" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFTRAININGNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>

                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Course">
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCourseName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREQUESTEDCOURSES").ToString() %>'></telerik:RadLabel>
                                            <%-- <asp:ImageButton ID="imgCourseName" Visible="true" runat="server" ImageUrl="<%$ PhoenixTheme:images/te_view.png%>" CommandArgument='<%# Container.DataItemIndex %>'></asp:ImageButton>
                                    <eluc:ToolTip ID="ucToolTipCourseName" Width="450px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREQUESTEDCOURSES") %>' />--%>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="200px"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblDonebyHeader" runat="server" Text="To be done by"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblDoneby" Width="200px" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOBEDONEBYNAME") %>'></telerik:RadLabel>
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
                        <div>
                            <hr />
                            <h3>
                                <telerik:RadLabel ID="lblexpiryhead" runat="server" Text="Documents expire with in 1 year:"></telerik:RadLabel>
                            </h3>
                        </div>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvexpiredoc" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                            CellSpacing="0" GridLines="None" OnNeedDataSource="gvexpiredoc_NeedDataSource"
                            GroupingEnabled="false" EnableHeaderContextMenu="true"
                            AutoGenerateColumns="false">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" TableLayout="Fixed">
                                <HeaderStyle Width="102px" />
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
                                    <telerik:GridTemplateColumn HeaderText="Document Name">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lbldocuname" Width="200px" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Document Type">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lbltype" Width="200px" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Expiry Date">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblDoneby" Width="200px" runat="server" Visible="true" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDCUMENTEXPIRY"))%>'></telerik:RadLabel>
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
                    </div>
                    <eluc:Status ID="ucStatus" runat="server" />
                </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
