<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreSuitabilityCheckWithDocument.aspx.cs" Inherits="CrewOffshore_CrewOffshoreSuitabilityCheckWithDocument" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Suitability Check</title>

    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
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
    <form id="frmCrewSuitabilityCheck" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" />

        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

        <eluc:TabStrip ID="CrewMenu" runat="server" OnTabStripCommand="CrewMenu_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>


        <eluc:TabStrip ID="CrewMenuGeneral" runat="server" OnTabStripCommand="CrewMenuGeneral_TabStripCommand"></eluc:TabStrip>

        <table runat="server" id="tblPersonalMaster" width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox Width="200px" runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox Width="200px" runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox Width="200px" runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td runat="server" id="tdempno">
                    <telerik:RadLabel ID="lblEmployeeNo" runat="server" Text="Employee Number"></telerik:RadLabel>
                </td>
                <td runat="server" id="tdempnum">
                    <telerik:RadTextBox Width="200px" runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                </td>
                <td colspan="3">
                    <telerik:RadTextBox Width="200px" runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <hr />
                </td>
            </tr>
        </table>
        <b>
            <telerik:RadLabel ID="lblsanction" runat="server" Text="This crew comes under Sanctions check" Font-Size="Large" ForeColor="Red"></telerik:RadLabel>
        </b>
        <br />
        <b>
            <telerik:RadLabel ID="Label1" runat="server" Text="Proposed Vessels"></telerik:RadLabel>
        </b>

            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
                <telerik:RadGrid RenderMode="Lightweight" ID="gvPlannedVessel" runat="server" Height="100px" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="gvPlannedVessel_NeedDataSource"
                    AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed" >
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
                            <telerik:GridButtonColumn Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <telerik:GridTemplateColumn HeaderText="Vessel Name">
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="250px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANNEDVESSELNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Plan Status">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPlanStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPDSTATUS") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Expected Join Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblExpectedJoinDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDEXPECTEDJOINDATE")) %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Proposed By">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="250px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblProposedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSEDBY") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="false" UseStaticHeaders="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadAjaxPanel>

        <table width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblRank1" runat="server" Text=" Rank"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox Width="200px" ID="ddlRank" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                        DataTextField="FLDRANKNAME" DataValueField="FLDRANKID" AutoPostBack="true" OnTextChanged="ddlRank_TextChanged">
                    </telerik:RadComboBox>
                </td>

                <td>
                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems="true"
                        VesselsOnly="true" AssignedVessels="true" AutoPostBack="true" OnTextChangedEvent="ucVessel_TextChangedEvent" Width="200px" />

                </td>
                <td>
                    <telerik:RadLabel ID="lblvesseltype" runat="server" Text="Vessel Type"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox Width="200px" runat="server" ID="txtvesseltype" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblMatrix" runat="server" Text="Charter"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox Width="200px" runat="server" ID="txtchartername" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblflag" runat="server" Text="Flag"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox Width="200px" runat="server" ID="txtflagname" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblCompany" runat="server" Text="Company"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox Width="200px" runat="server" ID="txtcompanyname" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblExpectedJoiningDate" runat="server" Text=" Expected Joining Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucDate" runat="server" CssClass="input_mandatory" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblOffsigner" runat="server" Text="Off-signer"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlOffSigner" runat="server" Width="200px"
                        AppendDataBoundItems="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblShowAll" runat="server" Text="Show All"></telerik:RadLabel>
                </td>
                <td colspan="5">
                    <asp:CheckBox ID="chkShowAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkShowAll_CheckedChanged" />
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblCharternameselect" runat="server" Text="Charter Name"></asp:Literal></td>
                <td>
                    <telerik:RadComboBox ID="ddlchartername" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlchartername_SelectedIndexChanged"
                        Filter="Contains" EmptyMessage="Type to select charter" MarkFirstMatch="true" Width="200px">
                    </telerik:RadComboBox>
                </td>
                <td>
                    <asp:Literal ID="Literal1" runat="server" Text="Max Tour Days"></asp:Literal></td>
                <td>
                    <telerik:RadTextBox Width="200px" runat="server" ID="txtmaxtourday" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>

                </td>
                <td colspan="2"></td>

            </tr>
            <tr>
                <td>
                    <telerik:RadLabel Visible="false" ID="lblVesselType1" runat="server" Text="Vessel Type"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:VesselType Visible="false" ID="ucVesselType" runat="server" AppendDataBoundItems="true" CssClass="readonlytextbox"
                        Enabled="false" />
                </td>
                <td></td>
            </tr>
        </table>
        <telerik:RadLabel ID="lblNote" runat="server" CssClass="guideline_text">
            Note: Please note validity of document checked for "contract + 1 month" starting from the expected joining date.
        </telerik:RadLabel>
        <br />
        <telerik:RadLabel ID="lblNote1" runat="server" CssClass="guideline_text">
            Please select vessel to check if the document is waived.
        </telerik:RadLabel>

            <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server">
                <telerik:RadGrid RenderMode="Lightweight" ID="gvSuitability" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                    CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvSuitability_NeedDataSource"
                    OnItemDataBound="gvSuitability_ItemDataBound"
                    OnItemCommand="gvSuitability_ItemCommand"
                    AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                        AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
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
                        <GroupByExpressions>
                            <telerik:GridGroupByExpression>
                                <SelectFields>
                                    <telerik:GridGroupByField FieldName="FLDCATEGORY" FieldAlias="Category" SortOrder="Ascending" />
                                </SelectFields>
                                <GroupByFields>
                                    <telerik:GridGroupByField FieldName="FLDCATEGORY" SortOrder="Ascending" />
                                </GroupByFields>

                            </telerik:GridGroupByExpression>
                        </GroupByExpressions>
                        <Columns>
                            <telerik:GridButtonColumn Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <telerik:GridTemplateColumn HeaderText="Category" Visible="false" Groupable="true" ColumnGroupName="FLDCATEGORY" HeaderStyle-Width="100px">

                                <ItemStyle Wrap="true" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORY") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="100px">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Stage" HeaderStyle-Width="100px">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblStage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAGENAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Required Document" HeaderStyle-Width="200px">
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblReqDocumentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUIREDDOC") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn Visible="false" HeaderText="Charter Name" HeaderStyle-Width="200px">
                                <ItemStyle Wrap="true" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblcharterid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHARTERID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblCharterName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHARTERNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Available Document" HeaderStyle-Width="200px">
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDocumentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblMissingYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMISSINGYN") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblExpiredYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIREDYN") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblVerifiedYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERIFIEDYN") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblAuthenticatedYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAUTHENTICATEDYN") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblAttachmenttype" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTACHMENTTYPE") %>'></telerik:RadLabel>

                                    <telerik:RadLabel ID="lblDocumentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblDocumentType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'></asp:LinkButton>
                                    <telerik:RadLabel ID="lblWaivedDocumentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAIVEDDOCID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblStageid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAGEID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblPlanStatus" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANSTATUS") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblProposalStageid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSALSTAGEID") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Document Status" HeaderStyle-Width="100px">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Can be</br> waived later" Visible="false" HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="90px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkCanbeWaivedYN" runat="server" Enabled="false" AutoPostBack="true" OnCheckedChanged="chkCanbeWaivedYN_CheckedChanged"
                                        Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDCANBEWAIVED")).ToString().Equals("1")?true:false %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Waive</br> Required Y/N" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkWaivedYN" runat="server" Enabled="false" AutoPostBack="true"
                                        OnCheckedChanged="chkWaivedYN_CheckedChanged" Checked='<%# General.GetNullableInteger((DataBinder.Eval(Container, "DataItem.FLDISWAIVED")).ToString())>0 ?true:false %>' />
                                    <eluc:ToolTip ID="ucToolTipDate" runat="server" Text='<%# "Waive requested by : " + DataBinder.Eval(Container,"DataItem.FLDREQUESTEDBY") +"<br/>Waive requested Date :  " + DataBinder.Eval(Container,"DataItem.FLDREQUESTEDDATE","{0:dd/MMM/yyyy}") %>' TargetControlId="chkWaivedYN" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Reason for</br> Waive requested" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px">
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblReason" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.FLDWAIVINGREASON")) %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox ID="txtReason" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.FLDWAIVINGREASON")) %>' Width="100%"></telerik:RadTextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Waive requested by" Visible="false" HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblWaiveBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTEDBY") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Waive requested Date" Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblWaivedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDREQUESTEDDATE")) %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Waive Approve" HeaderStyle-Width="50px">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblwaivedocid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAIVEDDOCID") %>'></telerik:RadLabel>
                                    <asp:LinkButton runat="server" AlternateText="Approve"
                                        CommandName="APPROVE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdApprove" Visible="false"
                                        ToolTip="Waive">
                                        <span class="icon"><i class="fas fa-award"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Waived Y/N" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkWaivedconfirmYN" runat="server" Enabled="false"
                                        Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDISWAIVED")).ToString().Equals("1")?true:false %>' />
                                    <eluc:ToolTip ID="ucWaiveToolTipDate" runat="server" Text='<%# "Waived by : " + DataBinder.Eval(Container,"DataItem.FLDWAIVEDUSERNAME") +"<br/>Waived Date :  " + DataBinder.Eval(Container,"DataItem.FLDWAIVEDDATE","{0:dd/MMM/yyyy}") %>' TargetControlId="chkWaivedconfirmYN" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Waived by" Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblWaiveitemBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAIVEDUSERNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Waived Date" Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblWaiveditemDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDWAIVEDDATE")) %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Expiry Date" HeaderStyle-Width="80px">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblExpiryDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY")) %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Issuing Authority" HeaderStyle-Width="100px">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblNationality" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNATIONALITY") %>'></telerik:RadLabel>
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
                                        CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel">
                                         <span class="icon"><i class="fa fa-trash"></i></span>
                                    </asp:LinkButton>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>

                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" EnableNextPrevFrozenColumns="true" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadAjaxPanel>
      
        <table cellpadding="1" cellspacing="1">
            <tr>
                <td>
                    <img id="Img1" src="<%$ PhoenixTheme:images/red.png%>" runat="server" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblDocsExpired" runat="server" CssClass="guideline_text">* Documents Expired & Missing</telerik:RadLabel>
                </td>
                <%--<td>
                            <img id="Img2" src="<%$ PhoenixTheme:images/blue.png%>" runat="server" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblDocsMissing" runat="server" CssClass="guideline_text">* Documents Missing</telerik:RadLabel>
                        </td>--%>
            </tr>
        </table>

        <b>
            <telerik:RadLabel ID="lblRankExpNote" runat="server" Text="Rank experience required in ANY of the below ranks"></telerik:RadLabel>
        </b>

            <telerik:RadAjaxPanel ID="RadAjaxPanel3" runat="server">
                <telerik:RadGrid RenderMode="Lightweight" ID="gvRankExp" runat="server" Height="150px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvRankExp_NeedDataSource"
                    OnItemDataBound="gvRankExp_ItemDataBound"
                    OnItemCommand="gvRankExp_ItemCommand"
                    AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed" >
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
                            <telerik:GridButtonColumn Text="DoubleClick" CommandName="Edit" Visible="false" />

                            <telerik:GridTemplateColumn HeaderText="Stage">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblStage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAGENAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Rank">
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblShortfall" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTFALL") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblDocumentType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblWaivedDocumentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAIVEDDOCID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblStageid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAGEID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblPlanStatus" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANSTATUS") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblProposalStageid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSALSTAGEID") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Can be</br> waived later" Visible="false" HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="90px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkCanbeWaivedYN" runat="server" Enabled="false" AutoPostBack="true" OnCheckedChanged="chkCanbeWaivedYNRankExp_CheckedChanged"
                                        Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDCANBEWAIVED")).ToString().Equals("1")?true:false %>' />

                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Waive</br> Required Y/N" HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkWaivedYN" runat="server" Enabled="false" AutoPostBack="true"
                                        OnCheckedChanged="chkWaivedYNRankExp_CheckedChanged" Checked='<%# General.GetNullableInteger((DataBinder.Eval(Container, "DataItem.FLDISWAIVED")).ToString())>0 ?true:false %>' />
                                    <eluc:ToolTip ID="ucToolTipDate" runat="server" Text='<%# "Waive requested by : " + DataBinder.Eval(Container,"DataItem.FLDREQUESTEDBY") +"<br/>Waive requested Date :  " + DataBinder.Eval(Container,"DataItem.FLDREQUESTEDDATE","{0:dd/MMM/yyyy}") %>' TargetControlId="chkWaivedYN" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Reason for</br> Waive requested" HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblReason" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.FLDWAIVINGREASON")) %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox ID="txtReason" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.FLDWAIVINGREASON")) %>'></telerik:RadTextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Waive requested by" Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblWaiveBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTEDBY") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Waive requested Date" Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblWaivedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDREQUESTEDDATE")) %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Waive Approve">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblwaivedocid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAIVEDDOCID") %>'></telerik:RadLabel>
                                    <asp:LinkButton runat="server" AlternateText="Approve"
                                        CommandName="APPROVE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdApprove" Visible="false"
                                        ToolTip="Waive">
                                         <span class="icon"><i class="fas fa-award"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Waived Y/N">
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="90px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkWaivedconfirmYN" runat="server" Enabled="false"
                                        Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDISWAIVED")).ToString().Equals("1")?true:false %>' />
                                    <eluc:ToolTip ID="ucWaiveToolTipDate" runat="server" Text='<%# "Waived by : " + DataBinder.Eval(Container,"DataItem.FLDWAIVEDUSERNAME") +"<br/>Waived Date :  " + DataBinder.Eval(Container,"DataItem.FLDWAIVEDDATE","{0:dd/MMM/yyyy}") %>' TargetControlId="chkWaivedconfirmYN" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Waived by" Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblWaiveitemBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAIVEDUSERNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Waived Date" Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblWaiveditemDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDWAIVEDDATE")) %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Experience Required (in months)">
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRankExpReqd" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKEXPREQUIRED") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Combined Experience held (in months)">
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRankExp" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPERIENCE") %>'></telerik:RadLabel>
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
                                        CommandName="CANCEL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel">
                                <span class="icon"><i class="fa fa-trash"></i></span>
                                    </asp:LinkButton>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>

                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" EnableNextPrevFrozenColumns="true" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadAjaxPanel>
        <b>
            <telerik:RadLabel ID="lblVesselTypeExpNote" runat="server" Text="Vessel Type experience required in ANY of the below vessel types"></telerik:RadLabel>
        </b>

            <telerik:RadAjaxPanel ID="RadAjaxPanel4" runat="server">
                <telerik:RadGrid RenderMode="Lightweight" ID="gvVesselTypeExp" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                    CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvVesselTypeExp_NeedDataSource"
                    OnItemDataBound="gvVesselTypeExp_ItemDataBound"
                    OnItemCommand="gvVesselTypeExp_ItemCommand"
                    AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed" >
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
                            <telerik:GridButtonColumn Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <telerik:GridTemplateColumn HeaderText="Stage">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblStage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAGENAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Vessel Type">
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblVesselType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEDESCRIPTION") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblShortfall" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTFALL") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblDocumentType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblWaivedDocumentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAIVEDDOCID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblStageid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAGEID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblPlanStatus" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANSTATUS") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblProposalStageid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSALSTAGEID") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Can be</br> waived later" Visible="false" HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="90px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkCanbeWaivedYN" runat="server" Enabled="false" AutoPostBack="true" OnCheckedChanged="chkCanbeWaivedYNVTExp_CheckedChanged"
                                        Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDCANBEWAIVED")).ToString().Equals("1")?true:false %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Waive</br> Required Y/N" HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkWaivedYN" runat="server" Enabled="false" AutoPostBack="true"
                                        OnCheckedChanged="chkWaivedYNVTExp_CheckedChanged" Checked='<%# General.GetNullableInteger((DataBinder.Eval(Container, "DataItem.FLDISWAIVED")).ToString())>0 ?true:false %>' />
                                    <eluc:ToolTip ID="ucToolTipDate" runat="server" Text='<%# "Waive requested by : " + DataBinder.Eval(Container,"DataItem.FLDREQUESTEDBY") +"<br/>Waive requested Date :  " + DataBinder.Eval(Container,"DataItem.FLDREQUESTEDDATE","{0:dd/MMM/yyyy}") %>' TargetControlId="chkWaivedYN" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Reason for</br> Waive requested" HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblReason" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.FLDWAIVINGREASON")) %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox ID="txtReason" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.FLDWAIVINGREASON")) %>'></telerik:RadTextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Waive requested by" Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblWaiveBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTEDBY") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Waive requested Date" Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblWaivedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDREQUESTEDDATE")) %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Waive Approve">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblwaivedocid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAIVEDDOCID") %>'></telerik:RadLabel>
                                    <asp:LinkButton runat="server" AlternateText="Approve"
                                        CommandName="APPROVE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdApprove" Visible="false"
                                        ToolTip="Waive">
                                         <span class="icon"><i class="fas fa-award"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Waived Y/N">
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="90px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkWaivedconfirmYN" runat="server" Enabled="false"
                                        Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDISWAIVED")).ToString().Equals("1")?true:false %>' />
                                    <eluc:ToolTip ID="ucWaiveToolTipDate" runat="server" Text='<%# "Waived by : " + DataBinder.Eval(Container,"DataItem.FLDWAIVEDUSERNAME") +"<br/>Waived Date :  " + DataBinder.Eval(Container,"DataItem.FLDWAIVEDDATE","{0:dd/MMM/yyyy}") %>' TargetControlId="chkWaivedconfirmYN" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Waived by" Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblWaiveitemBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAIVEDUSERNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Waived Date" Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblWaiveditemDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDWAIVEDDATE")) %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Experience Required (in months)">
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblVTExpReqd" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPEEXPREQUIRED") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Combined Experience held (in months)">
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblVesselTypeExp" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPERIENCE") %>'></telerik:RadLabel>
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
                                        CommandName="CANCEL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel">
                                 <span class="icon"><i class="fa fa-trash"></i></span>
                                    </asp:LinkButton>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" EnableNextPrevFrozenColumns="true" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadAjaxPanel>

    </form>
</body>
</html>

