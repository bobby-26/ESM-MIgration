<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreContractRenewal.aspx.cs" Inherits="CrewOffshore_CrewOffshoreContractRenewal" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BankAccount" Src="~/UserControls/UserControlEmployeeBankAccount.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%; position: absolute;">

                <%--<eluc:Title runat="server" ID="ucTitle" Text="Appointment Letter" ShowMenu="false" />--%>
                <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

                <eluc:TabStrip ID="CrewMenu" runat="server" Title="Appointment Letter" OnTabStripCommand="CrewMenu_TabStripCommand"></eluc:TabStrip>

                <table runat="server" id="tblPersonalMaster" width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
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
                            <telerik:RadLabel ID="lblDOB" runat="server" Text="DOB"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucDOB" runat="server" CssClass="readonlytextbox" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblPlaceofBirth" runat="server" Text="Place of Birth"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtPlaceOfBirth" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblRankNationality" runat="server" Text="Rank / Nationality"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtRank" runat="server" CssClass="input readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            /
                            <telerik:RadTextBox ID="txtNationality" runat="server" CssClass="input readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblSeamanBookNo" runat="server" Text="Seaman Book No"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtSeamanBook" runat="server" CssClass="input readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblAddress" runat="server" Text="Address"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadTextBox ID="txtAddress" runat="server" CssClass="readonlytextbox" Width="670px"
                                ReadOnly="true">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVessel" runat="server" Text="Proposed Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtVessel" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblRegOwner" runat="server" Text="Reg. Owner"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtRegOwner" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVesselAddress" runat="server" Text="Address"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadTextBox ID="txtVesselAddress" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="670px"></telerik:RadTextBox>
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblIMONo" runat="server" Text="IMO No."></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtIMONo" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td colspan="3"></td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblPlaceofEngagement" runat="server" Text="Place of Engagement"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:SeaPort ID="ddlSignOnSeaPort" runat="server" AppendDataBoundItems="true" Width="150px" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblPlaceOfRepatriation" runat="server" Text="Place of Repatriation"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:SeaPort ID="ddlSignOffPort" runat="server" AppendDataBoundItems="true" Width="150px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblContractStartDate" runat="server" Text="Contract commencement date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucContractStartDate" runat="server" ReadOnly="true"/>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblContractCancellationDate" runat="server" Text="Sign on date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucsignonDate" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblDailyWages" runat="server" Text="Daily Wages"></telerik:RadLabel>
                            <telerik:RadLabel ID="lblcurrency" runat="server"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="txtDailyWages" runat="server" CssClass="readonlytextbox" IsInteger="true" ReadOnly="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblContractPeriodDays" runat="server" Text="Tenure (Days)"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="txtContractPeriodDays" runat="server" CssClass="readonlytextbox" IsInteger="true" ReadOnly="true" />
                            <telerik:RadLabel ID="lblcontractrenewdays" runat="server" Text="Renew/Extend (Days)"></telerik:RadLabel>
                            <eluc:Number ID="txtrenewdays" runat="server" IsInteger="true"  CssClass="input_mandatory"  AutoPostBack="true" OnTextChangedEvent="txtrenewdays_TextChangedEvent" />
                            <telerik:RadLabel ID="lblPlusMinus" runat="server" Text="+/-"></telerik:RadLabel>
                            <eluc:Number ID="txtPlusMinusPeriod" runat="server" CssClass="readonlytextbox" IsInteger="true" ReadOnly="true" />
                            <telerik:RadLabel ID="lblPlusMinusDays" runat="server" Text="(Days)"></telerik:RadLabel>
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblDPAllowance" runat="server" Text="DP Allowance"></telerik:RadLabel>
                            <telerik:RadLabel ID="lblcurrencyAllowance" runat="server"></telerik:RadLabel>
                            <telerik:RadLabel ID="lblCurrencyid" Visible="false" runat="server"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="txtDPAllowance" runat="server" CssClass="readonlytextbox" IsInteger="true" ReadOnly="true" />
                        </td>
                        <td colspan="2">
                            <b>
                                <telerik:RadLabel ID="lblNote" runat="server" EnableViewState="false" Text="Note : Tenure commences from the date of sign on."
                                    BorderStyle="None" ForeColor="Blue">
                                </telerik:RadLabel>
                            </b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblSignOffTravelDays" runat="server" Text="SignOff Travel Days"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="txtSignOffTravelDays" runat="server" IsInteger="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblBankAccount" runat="server" Text="Bank Account"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:BankAccount ID="ucBankAccount" runat="server" AppendDataBoundItems="true" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblRank1" runat="server" Text=" Rank"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox Width="200px" runat="server" ID="txtrankname" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>

                        </td>

                        <td>
                            <telerik:RadLabel ID="RadLabel1" runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox Width="200px" runat="server" ID="txtvesselname" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>


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
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td colspan="5"></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblCharternameselect" runat="server" Text="Charter Name"></asp:Literal></td>
                        <td>
                            <telerik:RadComboBox ID="ddlchartername" runat="server" AutoPostBack="true"
                                Filter="Contains" EmptyMessage="Type to select charter" MarkFirstMatch="true" Width="200px">
                            </telerik:RadComboBox>
                        </td>
                        <td>Extended Date</td>
                        <td><eluc:Date ID="txtExtendeddate" ReadOnly="true" runat="server" /></td>
                        <td colspan="2"></td>
                    </tr>

                </table>
                <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server">
                <telerik:RadGrid RenderMode="Lightweight" ID="gvSuitability" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                    CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvSuitability_NeedDataSource"
                    OnItemDataBound="gvSuitability_ItemDataBound"
                    AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                        AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center" CommandItemDisplay="Top">
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
                                    <asp:CheckBox ID="chkCanbeWaivedYN" runat="server" Enabled="false" AutoPostBack="true" 
                                        Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDCANBEWAIVED")).ToString().Equals("1")?true:false %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Waive</br> Required Y/N" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkWaivedYN" runat="server" Enabled="false" 
                                        Checked='<%# General.GetNullableInteger((DataBinder.Eval(Container, "DataItem.FLDISWAIVED")).ToString())>0 ?true:false %>' />
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
                            <telerik:GridTemplateColumn Visible="false">
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
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadAjaxPanel>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
