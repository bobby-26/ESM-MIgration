<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreProposal.aspx.cs" Inherits="CrewOffshoreProposal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlTooltip.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Proposal</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmAppraisalQuestion" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>

        <div class="navigation" id="Div1" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
         

            <eluc:TabStrip ID="CrewMenuGeneral" runat="server" Title="Proposal" OnTabStripCommand="CrewMenuGeneral_TabStripCommand"></eluc:TabStrip>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

            <br />
            <div id="divInput" runat="server">
                <table id="tblProposal" runat="server" width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Vessel runat="server" ID="ucVessel" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                VesselsOnly="true" AssignedVessels="true" AutoPostBack="true" Enabled="false" OnTextChangedEvent="SetVesselType" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true" CssClass="readonlytextbox"
                                Enabled="false" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblRank1" runat="server" Text=" Rank"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlRank" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" Enabled="false"
                                DataTextField="FLDRANKNAME" DataValueField="FLDRANKID" AutoPostBack="true" OnTextChanged="ddlRank_Changed"
                                MarkFirstMatch="true" Filter="Contains" EmptyMessage="Type to select Rank">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblExpectedJoiningDate" runat="server" Text=" Expected Joining Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucDate" runat="server" CssClass="input_mandatory" Enabled="false" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblMatrix" runat="server" Text="Training Matrix"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlTrainingMatrix" runat="server" Width="255px" CssClass="input_mandatory"
                                AppendDataBoundItems="true" Enabled="false" MarkFirstMatch="true" Filter="Contains" EmptyMessage="Type to select Matrix">
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblOffsigner" runat="server" Text="Off-signer"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlOffSigner" runat="server" Width="242px" CssClass="input"
                                AppendDataBoundItems="true" Enabled="false" MarkFirstMatch="true" Filter="Contains" EmptyMessage="Type to select Off-signer">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <telerik:RadLabel ID="lblSalagreed" runat="server" Text="Daily Rate"></telerik:RadLabel>
                            <telerik:RadLabel ID="lblcurrencytype" runat="server"></telerik:RadLabel>
                        </td>
                        <td colspan="4">
                            <eluc:Number ID="txtSalagreed" runat="server" CssClass="input_mandatory" IsInteger="true" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <telerik:RadLabel ID="lblDPAllowance" runat="server" Text="SOCSO Contribution"></telerik:RadLabel>
                            <telerik:RadLabel ID="lblcurrencytypedp" runat="server"></telerik:RadLabel>
                            <telerik:RadLabel ID="lblcurrencytypeid" Visible="false" runat="server"></telerik:RadLabel>
                        </td>
                        <td colspan="4">
                            <eluc:Number ID="txtDPAllowance" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <telerik:RadLabel ID="lblEPFcontribution" runat="server" Text="EPF Contribution (%)"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <eluc:Number ID="txtEPFcontribution" runat="server" CssClass="input_mandatory" OnTextChangedEvent="txtSearch_TextChanged" AutoPostBack="true" IsInteger="true" />
                            <telerik:RadLabel ID="lblPercentage" runat="server" Text="(%)"></telerik:RadLabel>
                            <eluc:Number ID="txtEPFcontribution2" runat="server" CssClass="input_mandatory" IsInteger="true" />
                            <telerik:RadLabel ID="lblPercentageAmount" runat="server" Text="(Amount)"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <telerik:RadLabel ID="lblContractPeriod" runat="server" Text="Tenure (Days)"></telerik:RadLabel>
                        </td>
                        <td colspan="4">
                            <eluc:Number ID="txtContractPeriod" runat="server" CssClass="input_mandatory" IsInteger="true" />
                            <telerik:RadLabel ID="lblPlusMinus" runat="server" Text="+/-"></telerik:RadLabel>
                            <eluc:Number ID="txtPlusMinusPeriod" runat="server" CssClass="input_mandatory" IsInteger="true" />
                            <telerik:RadLabel ID="lblPlusMinusDays" runat="server" Text="(Days)"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr runat="server" visible="false">
                        <td colspan="2">
                            <telerik:RadLabel ID="lblTrainingNeeds" runat="server" Text="Has no outstanding training needs?"></telerik:RadLabel>
                        </td>
                        <td colspan="4">
                            <asp:RadioButtonList ID="rblTrainingNeeds" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                <asp:ListItem Text="N/A" Value="2"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr runat="server" visible="false">
                        <td colspan="2">
                            <telerik:RadLabel ID="lblPrevAppraisal" runat="server" Text="Previous appraisals are satisfactory?"></telerik:RadLabel>
                        </td>
                        <td colspan="4">
                            <asp:RadioButtonList ID="rblPreviousAppraisal" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                <asp:ListItem Text="N/A" Value="2"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr runat="server" visible="false">
                        <td colspan="2">
                            <telerik:RadLabel ID="lblRefCheck" runat="server" Text="Reference checks are satisfactory?"></telerik:RadLabel>
                        </td>
                        <td colspan="4">
                            <asp:RadioButtonList ID="rblRefCheck" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                <asp:ListItem Text="N/A" Value="2"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                        </td>
                        <td colspan="4">
                            <telerik:RadTextBox ID="txtRemaks" runat="server" TextMode="MultiLine" Width="400px" Height="30px"
                                CssClass="input_mandatory">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <div id="div3" style="position: relative; z-index: +1;">
                                <%-- <asp:GridView ID="gvProposalCheckList" runat="server" AutoGenerateColumns="False"
                                    Font-Size="11px" Width="100%" CellPadding="3" OnRowDataBound="gvProposalCheckList_RowDataBound"
                                    ShowHeader="true" ShowFooter="true" EnableViewState="true" OnRowCommand="gvProposalCheckList_RowCommand">
                                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                    <RowStyle Height="10px" />--%>
                                <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server">
                                    <telerik:RadGrid RenderMode="Lightweight" ID="gvProposalCheckList" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvProposalCheckList_NeedDataSource"
                                        OnItemCommand="gvProposalCheckList_ItemCommand"
                                        OnItemDataBound="gvProposalCheckList_ItemDataBound"
                                        AutoGenerateColumns="false">
                                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                            AutoGenerateColumns="false" TableLayout="Fixed" >
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
                                                <telerik:GridButtonColumn Text="DoubleClick" CommandName="Edit" Visible="false" />
                                                <telerik:GridTemplateColumn HeaderText="Checklist">
                                                    <HeaderStyle Width="250px" />
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblQuestion" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblDocumentType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblWaivedDocumentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAIVEDDOCID") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Answer">
                                                    <HeaderStyle Width="200px" />
                                                    <ItemTemplate>
                                                        <asp:RadioButtonList ID="rblAnswer" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblAnswer_SelectedIndexChanged">
                                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="N/A" Value="2"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Waived Y/N">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkWaivedYN" runat="server" AutoPostBack="true" OnCheckedChanged="chkWaivedYNProposal_CheckedChanged"
                                                            Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDISWAIVED")).ToString().Equals("1")?true:false %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Reason">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblReason" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.FLDWAIVINGREASON")) %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <telerik:RadTextBox ID="txtReason" runat="server" CssClass="input" Text='<%# (DataBinder.Eval(Container, "DataItem.FLDWAIVINGREASON")) %>'></telerik:RadTextBox>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Waived by">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblWaiveBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAIVEDUSERNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Waived Date">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblWaivedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDWAIVEDDATE")) %>'></telerik:RadLabel>
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
                                                        <asp:LinkButton runat="server" AlternateText="Edit"
                                                            CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                                            ToolTip="Edit">
                                                     <span class="icon"><i class="fas fa-edit"></i></span>
                                                        </asp:LinkButton>

                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Save"
                                                            CommandName="UPDATE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                                            ToolTip="Save">
                                                    <span class="icon"><i class="fas fa-save"></i></span>
                                                        </asp:LinkButton>

                                                        <asp:LinkButton runat="server" AlternateText="Cancel"
                                                            CommandName="CANCEL" CommandArgument='<%# Container.DataSetIndex %>'
                                                            ID="cmdCancel" ToolTip="Cancel">
                                                    <span class="icon"><i class="fa fa-trash"></i></span>
                                                        </asp:LinkButton>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" EnableNextPrevFrozenColumns="true" ScrollHeight="" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </telerik:RadAjaxPanel>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <telerik:RadLabel ID="lblNote" runat="server" Font-Bold="true"></telerik:RadLabel>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <b><telerik:RadLabel ID="RadLabel1" runat="server" Text="Training Needs" Font-Bold="true"></telerik:RadLabel> </b>
            <br />

            <eluc:TabStrip ID="MenuCrewRecommendedCourse" runat="server" OnTabStripCommand="CrewRecommendedCourse_TabStripCommand"></eluc:TabStrip>

            <div id="div2" style="position: relative; z-index: +1">
                <%--  <asp:GridView ID="gvCrewRecommendedCourses" runat="server" AutoGenerateColumns="False"
                    Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvCrewRecommendedCourses_RowCommand"
                    OnRowDataBound="gvCrewRecommendedCourses_RowDataBound" OnRowCreated="gvCrewRecommendedCourses_RowCreated"
                    ShowFooter="false"
                    ShowHeader="true" EnableViewState="true" AllowSorting="true">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="10px" />--%>
                <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewRecommendedCourses" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvCrewRecommendedCourses_NeedDataSource"
                        OnItemCommand="gvCrewRecommendedCourses_ItemCommand"
                        OnItemDataBound="gvCrewRecommendedCourses_ItemDataBound"
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
                                <telerik:GridButtonColumn Text="DoubleClick" CommandName="Edit" Visible="false" />
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="50px" />
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblSerialHeader" runat="server" Text="No"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSno" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROW") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblDocumentType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblWaivedDocumentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAIVEDDOCID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblTrainingNeedID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGNEEDID") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblVesselHeader" runat="server" Text="Vessel"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblVessel" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <HeaderStyle Width="200px" />
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="130px"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblCategoryHeader" runat="server" Text="Category"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <HeaderStyle Width="200px" />
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="130px"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblSubCategoryHeader" runat="server" Text="Sub Category"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSubCategory" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <HeaderStyle Width="100px" />
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="130px"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblTraningHeader" runat="server" Text="Identified Training Need"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTrainingNeed" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGNEED") %>'></telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <HeaderStyle Width="100px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblImprovementHeader" runat="server" Text="Level of Improvement"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblLevelOfImprovement" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELOFIMPROVEMENTNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <HeaderStyle Width="100px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="40px"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblTypeofTrainingHeader" runat="server" Text="Type of Training"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTypeofTraining" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFTRAININGNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <HeaderStyle Width="100px" />
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblDonebyHeader" runat="server" Text="To be done by"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDoneby" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOBEDONEBYNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Waived Y/N">
                                    <HeaderStyle Width="50px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkWaivedYN" runat="server" AutoPostBack="true" OnCheckedChanged="chkWaivedYNTN_CheckedChanged"
                                            Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDISWAIVED")).ToString().Equals("2")?true:false %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Waiving Type">
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblWaivingType" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.FLDWAIVINGTYPENAME")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:RadioButtonList ID="rblWaivingType" runat="server" RepeatColumns="1" RepeatDirection="Vertical"
                                            OnSelectedIndexChanged="rblWaivingType_SelectedIndexChanged">
                                            <asp:ListItem Text="Permanent" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Before Sign-On" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Before next contract" Value="3"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Reason">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblReason" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.FLDWAIVINGREASON")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtReason" runat="server" CssClass="input" Text='<%# (DataBinder.Eval(Container, "DataItem.FLDWAIVINGREASON")) %>'></telerik:RadTextBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Waived by">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblWaiveBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAIVEDUSERNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Waived Date">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblWaivedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDWAIVEDDATE")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn Visible="false">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblDetailsHeader" runat="server" Text="Details of Training Imparted / Course Attended"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDetails" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILSOFTRAINING") %>'></telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn Visible="false">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblTrainerHeader" runat="server" Text="Name of Trainer / Institute"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTrainer" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFTRAINERINSTITUTE") %>'></telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn Visible="false">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblDateHeader" runat="server" Text="Date Completed"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCompletionDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATECOMPLETED")) %>'></telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblActionHeader" runat="server">Action</telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Edit"
                                            CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                            ToolTip="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>

                                        <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                            CommandName="ATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAtt"
                                            ToolTip="Upload Attachment"></asp:ImageButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save"
                                            CommandName="UPDATE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                            ToolTip="Save">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Cancel"
                                            CommandName="CANCEL" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
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
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="3" EnableNextPrevFrozenColumns="true" ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </telerik:RadAjaxPanel>

            </div>
            <br />
            <b><telerik:RadLabel ID="RadLabel2" runat="server" Text="Appraisal" Font-Bold="true"></telerik:RadLabel></b>

            <eluc:TabStrip ID="CrewAppraisal" runat="server" OnTabStripCommand="CrewAppraisal_TabStripCommand"></eluc:TabStrip>

            <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                <%-- <asp:GridView ID="gvAQ" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" ShowHeader="true" OnRowCommand="gvAQ_RowCommand"
                    OnRowDataBound="gvAQ_RowDataBound"
                    AllowSorting="true" OnSorting="gvAQ_Sorting"
                    EnableViewState="false">
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />--%>
                <telerik:RadAjaxPanel ID="RadAjaxPanel3" runat="server">
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvAQ" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="true" OnNeedDataSource="gvAQ_NeedDataSource"
                        OnItemCommand="gvAQ_ItemCommand"
                        OnItemDataBound="gvAQ_ItemDataBound"
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed">
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
                                <telerik:GridTemplateColumn HeaderText="Vessel Name">
                                    <HeaderStyle Width="150px" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblAppraisalId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCREWAPPRAISALID")%>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblVesselname" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDVESSELNAME")%>'></telerik:RadLabel>
                                        <asp:LinkButton ID="lbtnvesselname" Visible="false" runat="server" CommandArgument='<%# Container.DataSetIndex %>'
                                            Text='<%#DataBinder.Eval(Container, "DataItem.FLDVESSELNAME")%>' CommandName="SELECT"></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="From Date">
                                    <HeaderStyle Width="75px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblFromdate" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDFROMDATE", "{0:dd/MMM/yyyy}")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="To Date">
                                    <HeaderStyle Width="75px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTodate" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDTODATE", "{0:dd/MMM/yyyy}")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Appraisal Date">
                                    <HeaderStyle Width="75px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblAppraisaldate" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAPPRAISALDATE", "{0:dd/MMM/yyyy}")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Occassion For Report">
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblOccassion" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDOCCASSIONFORREPORT")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Total Score">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="90px"></HeaderStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblScore" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDTOTALSCORE")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Recommended Promotion Y/N">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px"></HeaderStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblPromotion" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROMOTIONYESNO")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Fit for Re-employment Y/N">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px"></HeaderStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblReemployment" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRECOMMENDEDSTATUSNAME")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="HOD Remarks">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblHODRemarks" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDHEADDEPTCOMMENT").ToString().Length>20 ? DataBinder.Eval(Container, "DataItem.FLDHEADDEPTCOMMENT").ToString().Substring(0, 20)+ "..." : DataBinder.Eval(Container, "DataItem.FLDHEADDEPTCOMMENT").ToString()%>'></telerik:RadLabel>
                                        <eluc:Tooltip ID="ucHODRemarks" runat="server" Width="200px" Text='<%#DataBinder.Eval(Container, "DataItem.FLDHEADDEPTCOMMENT")%>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Master Remarks">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblMasterRemarks" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDMASTERCOMMENT").ToString().Length>20 ? DataBinder.Eval(Container, "DataItem.FLDMASTERCOMMENT").ToString().Substring(0, 20)+ "..." : DataBinder.Eval(Container, "DataItem.FLDMASTERCOMMENT").ToString()%>'></telerik:RadLabel>
                                        <eluc:Tooltip ID="ucMasterRemarks" runat="server" Width="200px" Text='<%#DataBinder.Eval(Container, "DataItem.FLDMASTERCOMMENT")%>' />
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
                                        <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                            CommandName="Attachment" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAtt"
                                            ToolTip="Attachment"></asp:ImageButton>

                                        <asp:LinkButton runat="server" AlternateText="Appraisal Report"
                                            CommandName="APPRAISAL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAppraisal"
                                            ToolTip="Appraisal Report">
                                    <scan class="icon"><i class="fas fa-chart-bar"></i></scan>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Promotion Report"
                                            CommandName="PROMOTION" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdPromotion"
                                            ToolTip="Promotion Report">
                                    <scan class="icon"><i class="fas fa-chart-line"></i></scan>
                                        </asp:LinkButton>
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
                </telerik:RadAjaxPanel>

            </div>
        </div>

    </form>
</body>
</html>
