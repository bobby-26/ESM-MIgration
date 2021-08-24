<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCandidateEvaluationList.aspx.cs" Inherits="CrewCandidateEvaluationList" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PhoneNumber" Src="~/UserControls/UserControlPhoneNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Candidate Evaluation</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewLicence" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="CrewTab" runat="server" Title="Assessment"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <table id="table1" runat="server" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtFirstName" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtMiddleName" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtLastName" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox runat="server" ID="txtRank" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <hr />
            <eluc:TabStrip ID="MenuIntSummary" runat="server" OnTabStripCommand="MenuIntSummary_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvIntSum" runat="server" Height="45%" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvIntSum_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvIntSum_ItemDataBound"
                OnItemCommand="gvIntSum_ItemCommand" ShowFooter="false"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
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
                        <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInterviewId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINTERVIEWID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCrewPlanId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWPLANID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsPDAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISPDATTACHMENT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblInterviewStatus" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINTERVIEWSTATUS") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblPDDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                 <telerik:RadLabel ID="lblofferletterid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFERLETTERID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblofferletterYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFERLETTERYN") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkVessel" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>'
                                    Text=' <%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblShowPD" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHOWPD") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRankName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Exp. Join Date" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExpJoinDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDEXPECTEDJOINDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PD Status" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPDStatus" runat="server" ClientIDMode="AutoID" CssClass="tooltip" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPDSTATUS") %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipAddress" runat="server" Text='<%#HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDAPPROVALREMARKS").ToString()) %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Created Date" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCreatedDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Proposal Status" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblProposalStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSALSTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="De-plan" ID="cmdDelete" CommandName="DEPLAN" ToolTip="De-Plan" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-calendar-times"></i></span> 
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit" ToolTip="Edit" CommandName="EDIT" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attachment" ID="cmdAtt" ToolTip="Proposal Attachment" CommandName="Attachment" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="PD Attachment" ID="cmdPDAtt" CommandName="PdAttachment" ToolTip="Approved PD and Owner Approval Attachment" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Comment" ID="cmdComment" CommandName="Comment" ToolTip="Comments" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-comment-alt"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="De-Plan Remarks" ID="cmdDeplanRemark" CommandName="DEPLANREMARK" CommandArgument="<%# Container.DataSetIndex %>"
                                    ToolTip="De-Plan Remarks">                                   
                                <span class="icon"><i class="fas fa-glasses"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="PDStatus" ID="cmdApproavalRemarks" CommandName="APPROVALREMARKS" CommandArgument="<%# Container.DataSetIndex %>"
                                    ToolTip="Refresh Status Remarks">                                   
                                <span class="icon"><i class="fas fa-sync"></i></span> 
                                </asp:LinkButton>
                                 <asp:LinkButton runat="server" AlternateText="OfferLetter" ID="cdmOfferletter" CommandName="OFFERLETTER" CommandArgument="<%# Container.DataSetIndex %>"
                                    ToolTip="Offer Letter">  
                                                                      
                                <span class="icon"><i class="fa fa-plus-circle"></i></span> 
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br />
            <b>
                <telerik:RadLabel ID="lblPromotionCategory" runat="server" Text="Promotion Category"></telerik:RadLabel>
            </b>

            <eluc:TabStrip ID="MenuPromotionCategory" runat="server" OnTabStripCommand="MenuPromotionCategory_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvPromotionCategory" runat="server" Height="45%" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvPromotionCategory_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvPromotionCategory_ItemDataBound"
                OnItemCommand="gvPromotionCategory_ItemCommand" ShowFooter="True"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" Width="100%" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle />
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
                        <telerik:GridTemplateColumn HeaderText="S.No." AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSno" runat="server" Text='<%#Container.ItemIndex+1%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlCategory" runat="server" CssClass="input_mandatory" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="true">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="" Text="--Select--" />
                                        <telerik:RadComboBoxItem Value="1" Text="Category1" />
                                        <telerik:RadComboBoxItem Value="2" Text="Category2" />
                                        <telerik:RadComboBoxItem Value="3" Text="Category3" />
                                        <telerik:RadComboBoxItem Value="4" Text="Category4" />
                                    </Items>
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="With Effect From" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFromDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDEFFECTDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <eluc:Date ID="txtEffectDate" runat="server" CssClass="input_mandatory" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" ClientIDMode="AutoID" CssClass="tooltip" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString().Length > 50 ? DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString().Substring(0, 50) + "..." : DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString()%>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox runat="server" ID="txtAddRemarks" CssClass="gridinput_mandatory"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="User" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblUser" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCreatedDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ID="cmdAdd" CommandName="Add" ToolTip="Add New" Width="20px" Height="20px">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="false" CssClass=" RadGrid_Default rgPagerTextBox" />
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
