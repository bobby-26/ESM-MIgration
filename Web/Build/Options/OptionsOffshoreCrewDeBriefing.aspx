<%@ Page Language="C#" AutoEventWireup="True" CodeFile="OptionsOffshoreCrewDeBriefing.aspx.cs" Inherits="OptionsOffshoreCrewDeBriefing"
    MaintainScrollPositionOnPostback="true" %>

<%--<%@ Import Namespace="System.Data" %>--%>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonAddress" Src="../UserControls/UserControlCommonAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PhoneNumber" Src="../UserControls/UserControlPhoneNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MobileNumber" Src="../UserControls/UserControlMobileNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToBeDoneBy" Src="~/UserControls/UserControlOffshoreToBeDoneBy.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Occassion" Src="~/UserControls/UserControlOccassionForReport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title>De-Briefing</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
              <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
  <%--  <style type="text/css">
        .hidden {
            display: none;
        }

        .center {
            background: fixed !important;
        }
    </style>--%>
</head>

<body>
        <form id="frmRegistersState" runat="server" autocomplete="off">

            <asp:ScriptManager ID="radscript1" runat="server"></asp:ScriptManager>
            
            <telerik:RadWindowManager ID="phoenixPopup" runat="server" EnableShadow="true" ShowContentDuringLoad="true"
            Behaviors="Close, Move, Resize, Maximize, Minimize" DestroyOnClose="true" Opacity="99" Width="450" Height="400" Modal="true" VisibleStatusbar="false"   />
       
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            
                <eluc:TabStrip ID="FeedBackTabs" runat="server" OnTabStripCommand="FeedBackTabs_TabStripCommand"
                    TabStrip="false"></eluc:TabStrip>

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
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
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
                        <td>
                            <telerik:RadLabel ID="lblreviewd" Visible="false" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                </table>

                <table width="80%" cellpadding="1" cellspacing="1">
                    <tr>
                        <th nowrap>
                            <h3>
                                <telerik:RadLabel ID="lblavdetail" runat="server" Text="Availability and Contact Details"></telerik:RadLabel>
                            </h3>
                        </th>
                    </tr>
                </table>
                <table id="Table34" width="80%" style="color: black; font-size: small; font-family: Arial">
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
                        <td>
                            <eluc:Date ID="ucdoa" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                OnTextChangedEvent='txtDOA_TextChanged' />
                        </td>
                    </tr>

                </table>
                <table width="100%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td width="50%">
                            <b>
                                <telerik:RadLabel ID="lblPermanentAddress" runat="server" Text="Permanent Address"></telerik:RadLabel>
                            </b>
                        </td>

                        <td>
                            <b>
                                <telerik:RadLabel ID="lblPermanentContact" runat="server" Text="Permanent Contact"></telerik:RadLabel>
                            </b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <eluc:CommonAddress ID="PermanentAddress" runat="server" />
                        </td>
                        <td style="vertical-align: text-top;">
                            <table style="vertical-align: text-top;">
                                <tr>
                                    <td width="50%">
                                        <telerik:RadLabel ID="lblPhoneNumber" runat="server" Text="Phone Number"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:PhoneNumber ID="txtPhoneNumber" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblMobileNumber" runat="server" Text="Mobile Number"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:PhoneNumber ID="txtMobileNumber" IsMobileNumber="true" runat="server"
                                            CssClass="input" />
                                    </td>

                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblEMail" runat="server" Text="E-Mail"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtEmail" runat="server" Width="100%" CssClass="input_mandatory"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap>
                                        <telerik:RadLabel ID="lblPlaceofEngagement" runat="server" Text="Port of Engagement"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:SeaPort ID="ddlPortofEngagement" runat="server" CssClass="input" AppendDataBoundItems="true"
                                            Width="150px" />
                                    </td>
                                </tr>

                            </table>
                        </td>

                    </tr>
                </table>

                <hr />
               
                    <h3>
                        <telerik:RadLabel ID="lbldebriefinghead" runat="server" Text="De-Briefing:"></telerik:RadLabel>
                    </h3>

                <table id="Table2" width="80%" style="color: black; font-size: small; font-family: Arial">
                    <tr>
                        <td>We would also like feedback of your tenure onboard your last vessel and would appreciate if you could kindly fill up the Debriefing template below.
                                        <br />
                            This will allow us to address any issues or concerns that you may have as well as improve our systems
                        </td>
                    </tr>
                </table>
                <br />
            
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
                                    <telerik:RadLabel ID="lblGroup" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCATEGORYNAME")%>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Questions">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="99%"></HeaderStyle>
                                
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
                                                  <telerik:RadTextBox ID="txtComments" Visible="true" runat="server" CssClass="input" TextMode="MultiLine"
                                                                onkeyDown="checkTextAreaMaxLength(this,event,'500');" Width="300px" Height="30px"></telerik:RadTextBox>
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
                <br />              
                    <hr />
                    <h3>
                        <telerik:RadLabel ID="Literal1" runat="server" Text="Appraisal (Pending Seafarer Comment):"></telerik:RadLabel>
                    </h3>
                          
                <telerik:RadGrid RenderMode="Lightweight" ID="gvOffshorePappraisal" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                    CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvOffshorePappraisal_NeedDataSource"
                    OnItemCommand="gvOffshorePappraisal_ItemCommand"
                    OnItemDataBound="gvOffshorePappraisal_ItemDataBound"
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
                            <%-- <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>

                            <telerik:GridTemplateColumn HeaderText="Vessel">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblVessel" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="AppraisalId" Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblAppraisalId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPRAISALID") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                           <telerik:GridTemplateColumn HeaderText="CrewAppraisalId" Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCrewAppraisalId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWAPPRAISALID") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="From">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblFrom" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMDATE") %>'></telerik:RadLabel>
                                </ItemTemplate>

                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="To">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblTo" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTODATE") %>'></telerik:RadLabel>
                                </ItemTemplate>

                            </telerik:GridTemplateColumn>
                           <telerik:GridTemplateColumn HeaderText="Occassion">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                              
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblOccassion" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOCCASSIONFORREPORT") %>'></telerik:RadLabel>
                                </ItemTemplate>

                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Master Comment">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblMasterComment" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMASTERCOMMENT") %>'></telerik:RadLabel>
                                </ItemTemplate>

                            </telerik:GridTemplateColumn>
                           <telerik:GridTemplateColumn HeaderText="HOD Comment">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblHODComment" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHEADDEPTCOMMENT") %>'></telerik:RadLabel>
                                </ItemTemplate>

                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Seafarer Comment">
                                <ItemStyle Wrap="true" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadTextBox ID="txtSeafarerComment" TextMode="MultiLine" Width="98%" Height="35px" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSEAMANCOMMENT").ToString() %>'></telerik:RadTextBox>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                          <telerik:GridTemplateColumn HeaderText="Action">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                              
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Export XL" ImageUrl="<%$ PhoenixTheme:images/document_view.png %>"
                                        CommandName="VIEWACTIVITY" CommandArgument='<%# Container.DataSetIndex %>'
                                        ID="cmdCrewExport2XL" ToolTip="View De-Briefing"></asp:ImageButton>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl='<%$ PhoenixTheme:images/save.png%>'
                                        CommandName="UPDATE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
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
                    <hr />
                    <h3>
                        <telerik:RadLabel ID="lblPendingTrainingneeds" runat="server" Text="Training Needs:"></telerik:RadLabel>
                    </h3>
               
                <table id="Table11" width="100%" style="color: black; font-size: small; font-family: Arial">
                    <tr>
                        <td>The following Training needs have been identified for you.You are requested to kindly get in touch with your 
                                        <br />
                            designated manning office so that these can addressed promptly.
                        </td>
                    </tr>
                </table>
                <br />
              
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

                            <telerik:GridTemplateColumn HeaderText="Vessel">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblIsRaisedFromCBT" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISRAISEDFROMCBT") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblVessel" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                           <telerik:GridTemplateColumn HeaderText="Category">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>

                            </telerik:GridTemplateColumn>
                          <telerik:GridTemplateColumn HeaderText="Sub Category">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                              
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSubCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>

                            </telerik:GridTemplateColumn>   
                              <telerik:GridTemplateColumn HeaderText="Identified Training Need">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblTrainingNeed" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGNEED") %>'></telerik:RadLabel>
                                </ItemTemplate>

                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Level of Improvement">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblLevelOfImprovement" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELOFIMPROVEMENTNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>

                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Type of training">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                
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
                            <telerik:GridTemplateColumn HeaderText="To be done by">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="200px"></ItemStyle>
                                
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

                    <hr />
                    <h3>
                        <telerik:RadLabel ID="lblexpiryhead" runat="server" Text="Documents expire with in 1 year:"></telerik:RadLabel>
                    </h3>
               
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
                <input type="hidden" id="isouterpage" name="isouterpage" />
                <eluc:Status ID="ucStatus" runat="server" />
                
        </form>
   
</body>
</html>