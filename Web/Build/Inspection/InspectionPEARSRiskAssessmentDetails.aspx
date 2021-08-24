<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionPEARSRiskAssessmentDetails.aspx.cs" Inherits="InspectionPEARSRiskAssessmentDetails" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>

<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>PEARS RA</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            .checkboxstyle tbody tr td {
                width: 550px;
                vertical-align: top;                
            }
        </style>
        
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblGeneric" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuGeneral" runat="server" OnTabStripCommand="MenuGeneral_TabStripCommand"></eluc:TabStrip>
            <table width="100%" id="tblGeneric" CellSpacing="4" >
                <tr>
                    <td width="10%">
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td width="23%">
                        <telerik:RadTextBox ID="txtVessel" runat="server" Text="" ReadOnly="true" Width="200px"></telerik:RadTextBox>
                    </td>
                    <td width="10%">
                        <telerik:RadLabel ID="lblCompany" runat="server" Text="Company"></telerik:RadLabel>
                    </td>
                    <td width="23%">
                        <eluc:Company ID="ucCompany" runat="server" AppendDataBoundItems="true" Enabled="false" Width="200px" />
                    </td>
                    <td width="10%">
                        <telerik:RadLabel ID="lblRefNo" runat="server" Text="Ref.No"></telerik:RadLabel>
                    </td>
                    <td width="24%">
                        <telerik:RadTextBox ID="txtRefNo" runat="server" ReadOnly="true" Width="200px"></telerik:RadTextBox>
                    </td>
                </tr>
                                <tr>

                    <td>
                        <telerik:RadLabel ID="lblCreatedby" runat="server" Text="Created By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCreatedbyName" runat="server" ReadOnly="true" Width="49%"></telerik:RadTextBox>/
                        <telerik:RadTextBox ID="txtCreatedbyRank" runat="server" ReadOnly="true" Width="47%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReviewed" runat="server" Text="Reviewed By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRewiewedbyName" runat="server" ReadOnly="true" Width="49%"></telerik:RadTextBox>/
                        <telerik:RadTextBox ID="txtReviewedbyRank" runat="server" ReadOnly="true" Width="47%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblApproved" runat="server" Text="Approved By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtApprovedbyName" runat="server" ReadOnly="true" Width="49%"></telerik:RadTextBox>/
                        <telerik:RadTextBox ID="txtApprovedbyRank" runat="server" ReadOnly="true" Width="47%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblIssued" runat="server" Text="Issue By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtIssuedbyName" runat="server" ReadOnly="true" Width="98%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIssuedDate" runat="server" Text="Isssued Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucIssuedDate" runat="server" DatePicker="false" Enabled="false"/>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtStatus" runat="server" Text="" ReadOnly="true" Width="200px"></telerik:RadTextBox>
                    </td>

                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblIntendedWorkDate" runat="server" Text="Intended Work Date"></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <eluc:Date ID="ucIntendedWorkDate" CssClass="input_mandatory" runat="server" Width="200px" />
                    </td>
                    <td valign="top">
                        <telerik:RadLabel ID="lblTypeofActivity" runat="server" Text="Type of Activity"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtActivity" runat="server" Text="" Width="98%" Height="70px"
                            TextMode="MultiLine" MaxLength="200" CssClass="input_mandatory" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                    <td valign="top">
                        <telerik:RadLabel ID="lblActivitySite" runat="server" Text="Activity Worksite"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtActivitySite" runat="server" Text="" Width="98%" Height="70px"
                            TextMode="MultiLine" MaxLength="200" CssClass="input_mandatory" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblPPE" runat="server" Text="Summary of PPE"></telerik:RadLabel>
                    </td>
                    <td colspan="5">
                        <asp:CheckBoxList ID="cbRAPPE" runat="server" RepeatDirection="Vertical" RepeatColumns="4" DataTextField="FLDNAME" DataValueField="FLDMISCELLANEOUSID"
                             CssClass="checkboxstyle" Style="width: 99.5%; border-width: 1px; border-style: solid; border: 1px solid #c3cedd; ">
                        </asp:CheckBoxList>
                    </td>

                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblGroupMember" runat="server" Text="Work Group Members"></telerik:RadLabel>
                    </td>
                    <td colspan="5">
                        <asp:CheckBoxList ID="ChkgroupMem" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" RepeatColumns="4" AutoPostBack="false" 
                            CssClass="checkboxstyle" Style="width: 99.5%; border-width: 1px; border-style: solid; border: 1px solid #c3cedd" Enabled="false">
                        </asp:CheckBoxList>
                    </td>

                </tr>
            </table>
            <telerik:RadDockZone ID="RadDockZone7" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                <telerik:RadDock Width="100%" RenderMode="Lightweight" ID="RadDock7" runat="server" EnableAnimation="true" EnableDrag="false"
                    EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <TitlebarTemplate>
                        <telerik:RadLabel runat="server" ID="lblRA" Text="Activity Steps" Font-Bold="true"></telerik:RadLabel>
                    </TitlebarTemplate>
                    <ContentTemplate>
                        <eluc:TabStrip ID="MenuPEARSRA" runat="server" OnTabStripCommand="MenuPEARSRA_TabStripCommand"></eluc:TabStrip>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvPEARSRA" runat="server" AllowCustomPaging="true" AllowSorting="true"
                            CellSpacing="1" Width="130%" GridLines="None" EnableHeaderContextMenu="true" GroupingEnabled="false" ShowHeader="true" EnableViewState="false"
                            OnItemDataBound="gvPEARSRA_ItemDataBound" OnItemCommand="gvPEARSRA_ItemCommand" OnNeedDataSource="gvPEARSRA_NeedDataSource">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false">
                                <ColumnGroups>
                                    <telerik:GridColumnGroup Name="Hazard" HeaderText="Hazard" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                                    <telerik:GridColumnGroup Name="InitialRisk" HeaderText="Initial Risk" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                                    <telerik:GridColumnGroup Name="ResidualRisk" HeaderText="Residual Risk" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                                </ColumnGroups>
                                <NoRecordsTemplate>
                                    <table width="130%" border="0">
                                        <tr>
                                            <td align="center">
                                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Activity Steps" HeaderStyle-Width="18%" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblActivityStep" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITYSTEP")  %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblActivityStepid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPEARSRISKASSESSMENTACTIVITYSTEPID")  %>' Visible="false"></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Description</br>(Aspect)" ColumnGroupName="Hazard" HeaderStyle-Width="11%" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAspect" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASPECT")  %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Effect</br>(Impact)" ColumnGroupName="Hazard" HeaderStyle-Width="11%" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblImpact" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMPACT")  %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Persons at Risk" ColumnGroupName="Hazard" HeaderStyle-Width="11%" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblPersons" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERSONSINVOLVED")  %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Existing Controls" HeaderStyle-Width="17%" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblExisting" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXISTINGCONTROLS")  %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Hazard</br>Category" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <table>
                                                <tr>
                                                    <td>People</td>
                                                </tr>
                                                <tr>
                                                    <td>Environmental</td>
                                                </tr>
                                                <tr>
                                                    <td>Asset</td>
                                                </tr>
                                                <tr>
                                                    <td>Reputation</td>
                                                </tr>
                                                <tr>
                                                    <td>Schedule</td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Hazard</br>Severity" ColumnGroupName="InitialRisk" HeaderStyle-Width="4%" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="true" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblinitPplSeverity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINTIALPEOPLESEVERITYRATING")  %>'></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblinitEnvSeverity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINTIALENVIRONMENTSEVERITYRATING")  %>'></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblinitAssetSeverity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINTIALASSETSEVERITYRATING")  %>'></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblinitRepSeverity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINTIALREPUTATIONSEVERITYRATING")  %>'></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblinitSchSeverity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINTIALSCHEDULESEVERITYRATING")  %>'></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Likelihood" ColumnGroupName="InitialRisk" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="true" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblinitPplLoh" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINTIALPEOPLELOHRATING")  %>'></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblinitEnvLoh" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINTIALENVIRONMENTLOHRATING")  %>'></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblinitAssetLoh" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINTIALASSETLOHRATING")  %>'></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblinitRepLoh" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINTIALREPUTATIONLOHRATING")  %>'></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblinitSchLoh" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINTIALSCHEDULELOHRATING")  %>'></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Risk</br>Rating" ColumnGroupName="InitialRisk" HeaderStyle-Width="4%" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="true" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <table Width="100%" >
                                                <tr align="right">
                                                    <td>
                                                        <telerik:RadLabel ID="lblinitPpl" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINTIALPEOPLERISKRATING")  %>' Width="99%"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr align="right">
                                                    <td>
                                                        <telerik:RadLabel ID="lblinitEnv" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINTIALENVIRONMENTRISKRATING")  %>' Width="99%"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr align="right">
                                                    <td>
                                                        <telerik:RadLabel ID="lblinitAsset" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINTIALASSETRISKRATING")  %>' Width="99%"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr align="right">
                                                    <td>
                                                        <telerik:RadLabel ID="lblinitRep" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINTIALREPUTATIONRISKRATING")  %>' Width="99%"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr align="right">
                                                    <td>
                                                        <telerik:RadLabel ID="lblinitSch" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINTIALSCHEDULERISKRATING")  %>' Width="99%"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Additional Controls" HeaderStyle-Width="17%" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAdditional" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDITIONALCONTROLS")  %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Hazard</br>Category" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <table>
                                                <tr>
                                                    <td>People</td>
                                                </tr>
                                                <tr>
                                                    <td>Environmental</td>
                                                </tr>
                                                <tr>
                                                    <td>Asset</td>
                                                </tr>
                                                <tr>
                                                    <td>Reputation</td>
                                                </tr>
                                                <tr>
                                                    <td>Schedule</td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Hazard</br>Severity" ColumnGroupName="ResidualRisk" HeaderStyle-Width="4%" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="true" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblResPplSeverity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESIDULEPEOPLESEVERITYRATING")  %>'></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblResEnvSeverity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESIDULEENVIRONMENTSEVERITYRATING")  %>'></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblResAssetSeverity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESIDULEASSETSEVERITYRATING")  %>'></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblResRepSeverity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESIDULEREPUTATIONSEVERITYRATING")  %>'></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblResSchSeverity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESIDULESCHEDULESEVERITYRATING")  %>'></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Likelihood" ColumnGroupName="ResidualRisk" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="true" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblResPplLoh" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESIDULEPEOPLELOHRATING")  %>'></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblResEnvLoh" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESIDULEENVIRONMENTLOHRATING")  %>'></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblResAssetLoh" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESIDULEASSETLOHRATING")  %>'></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblResRepLoh" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESIDULEREPUTATIONLOHRATING")  %>'></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblResSchLoh" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESIDULESCHEDULELOHRATING")  %>'></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Risk</br>Rating" ColumnGroupName="ResidualRisk" HeaderStyle-Width="4%" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="true" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                           <table Width="100%" >
                                                <tr align="right">
                                                    <td>
                                                        <telerik:RadLabel ID="lblResPpl" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESIDULEPEOPLERISKRATING")  %>' Width="99%"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr align="right">
                                                    <td>
                                                        <telerik:RadLabel ID="lblResEnv" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESIDULEENVIRONMENTRISKRATING")  %>' Width="99%"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr align="right">
                                                    <td>
                                                        <telerik:RadLabel ID="lblResAsset" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESIDULEASSETRISKRATING")  %>' Width="99%"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr align="right">
                                                    <td>
                                                        <telerik:RadLabel ID="lblResRep" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESIDULEREPUTATIONRISKRATING")  %>' Width="99%"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr align="right">
                                                    <td>
                                                        <telerik:RadLabel ID="lblResSch" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESIDULESCHEDULERISKRATING")  %>' Width="99%"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Action" AllowFiltering="false" HeaderStyle-Width="10%">
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="CANCEL" ID="imgcancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </ContentTemplate>
                </telerik:RadDock>
            </telerik:RadDockZone>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
