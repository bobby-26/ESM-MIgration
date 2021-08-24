<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshorePendingWaivingDetail.aspx.cs" Inherits="CrewOffshorePendingWaivingDetail" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>


<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Suitability Check</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmCrewSuitabilityCheck" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%; position: absolute;">


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
                            <telerik:RadTextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td runat="server" id="tdempno">
                            <telerik:RadLabel ID="lblEmployeeNo" runat="server" Text="Employee Number"></telerik:RadLabel>
                        </td>
                        <td runat="server" id="tdempnum">
                            <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                </table>
                <b>
                    <telerik:RadLabel ID="Label1" runat="server" Visible="false" Text="Proposed Vessels"></telerik:RadLabel>
                </b>
                <div id="div1" style="position: relative; z-index: +1">
                    <%-- <asp:GridView ID="gvPlannedVessel" runat="server" AutoGenerateColumns="False" Font-Size="11px" Visible="false"
                        OnPreRender="gvPlannedVessel_PreRender" Width="100%" CellPadding="3" OnRowDataBound="gvPlannedVessel_RowDataBound"
                        ShowHeader="true" EnableViewState="true">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvPlannedVessel" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvPlannedVessel_NeedDataSource"
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
                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANNEDVESSELNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Plan Status">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblPlanStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPDSTATUS") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Expected Join Date">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblExpectedJoinDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDEXPECTEDJOINDATE")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Proposed By">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblProposedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSEDBY") %>'></telerik:RadLabel>
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
                <table width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblRank1" runat="server" Text=" Rank"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlRank" runat="server" AppendDataBoundItems="true" CssClass="readonlytextbox"
                                DataTextField="FLDRANKNAME" DataValueField="FLDRANKID" AutoPostBack="true" Width="150px" OnTextChanged="ddlRank_Changed">
                                <Items>
                                    <telerik:RadComboBoxItem Value="Dummy" Text="--Select--" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblMatrix" runat="server" Text="Training Matrix"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlTrainingMatrix" runat="server" Width="300px" CssClass="readonlytextbox" Enabled="false"
                                AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlTrainingMatrix_SelectedIndexChanged">
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Vessel runat="server" ID="ucVessel" CssClass="readonlytextbox" AppendDataBoundItems="true"
                                VesselsOnly="true" AssignedVessels="true" AutoPostBack="true" OnTextChangedEvent="SetVesselType" />
                            <%-- <asp:DropDownList ID="ucVessel" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                AutoPostBack="true" OnTextChanged="SetVesselType" DataTextField="FLDVESSELNAME"
                                DataValueField="FLDVESSELID">
                            </asp:DropDownList>--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblExpectedJoiningDate" runat="server" Text=" Expected Joining Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucDate" runat="server" CssClass="readonlytextbox" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblOffsigner" runat="server" Text="Off-signer" Visible="false"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlOffSigner" runat="server" Width="242px" Visible="false"
                                AppendDataBoundItems="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblShowAll" runat="server" Text="Show All" Visible="false"></telerik:RadLabel>
                        </td>
                        <td colspan="5">
                            <asp:CheckBox ID="chkShowAll" runat="server" Visible="false" AutoPostBack="true" CssClass="readonlytextbox" OnCheckedChanged="chkShowAll_CheckedChanged" />
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel Visible="false" ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:VesselType Visible="false" ID="ucVesselType" runat="server" AppendDataBoundItems="true" CssClass="readonlytextbox"
                                Enabled="false" />
                        </td>
                    </tr>
                </table>
                <telerik:RadLabel ID="lblNote" runat="server" CssClass="guideline_text">
                    Note: Please note validity of document checked for "contract + 1 month" starting from the expected joining date.
                </telerik:RadLabel>
                <br />
                <telerik:RadLabel ID="lblNote1" runat="server" CssClass="guideline_text">
                    Please select vessel to check if the document is waived.
                </telerik:RadLabel>

                <eluc:TabStrip ID="MenuCrewSuitabilityList" runat="server" OnTabStripCommand="CrewSuitabilityList_TabStripCommand"></eluc:TabStrip>

                <div id="divGrid" style="position: relative; z-index: +1">
                    <%-- <asp:GridView ID="gvSuitability" runat="server" AutoGenerateColumns="False" Font-Size="11px" OnRowCommand="gvSuitability_RowCommand"
                        Width="100%" CellPadding="3" OnRowDataBound="gvSuitability_RowDataBound" OnRowEditing="gvSuitability_RowEditing"
                        ShowHeader="true" EnableViewState="true" OnRowCancelingEdit="gvSuitability_RowCancelingEdit">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvSuitability" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvSuitability_NeedDataSource"
                        OnItemCommand="gvSuitability_ItemCommand"
                        OnItemDataBound="gvSuitability_ItemDataBound"
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

                                <telerik:GridTemplateColumn HeaderText="Category">
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="200px" VerticalAlign="Top"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORY") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Type">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Stage">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblStage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAGENAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Required Document">
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="130px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblReqDocumentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUIREDDOC") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Available Document">
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="130px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDocumentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblMissingYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMISSINGYN") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblExpiredYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIREDYN") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblVerifiedYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERIFIEDYN") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblAuthenticatedYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAUTHENTICATEDYN") %>'></telerik:RadLabel>
                                        <%--<telerik:RadLabel ID="lblFlagId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLAGID") %>'></telerik:RadLabel>--%>
                                        <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblAttachmenttype" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTACHMENTTYPE") %>'></telerik:RadLabel>
                                        <%--<asp:LinkButton ID="lnkVessel" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>'
                                        Text=' <%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:LinkButton>--%>
                                        <telerik:RadLabel ID="lblDocumentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblDocumentType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></telerik:RadLabel>
                                        <asp:LinkButton ID="lnkName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'></asp:LinkButton>
                                        <telerik:RadLabel ID="lblWaivedDocumentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAIVEDDOCID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblStageid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAGEID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblPlanStatus" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANSTATUS") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblProposalStageid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSALSTAGEID") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Document Status">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <%-- <telerik:GridTemplateColumn HeaderText="Can be</br> waived later" Visible="false" HeaderStyle-HorizontalAlign="Center">
                                    <ItemStyle Wrap="False" HorizontalAlign="Center"  Width="90px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkCanbeWaivedYN" runat="server" AutoPostBack="true" Enabled="false" OnCheckedChanged="chkCanbeWaivedYN_CheckedChanged"
                                          Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDCANBEWAIVED")).ToString().Equals("1")?true:false %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>--%>
                                <telerik:GridTemplateColumn HeaderText="Waive</br> Required Y/N" HeaderStyle-HorizontalAlign="Center">
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="90px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkWaivedYN" runat="server" Enabled="false"
                                            OnCheckedChanged="chkWaivedYN_CheckedChanged" Checked='<%# General.GetNullableInteger((DataBinder.Eval(Container, "DataItem.FLDISWAIVED")).ToString())>0 ?true:false %>' />
                                        <eluc:ToolTip ID="ucToolTipDate" runat="server" Text='<%# "Waive requested by : " + DataBinder.Eval(Container,"DataItem.FLDREQUESTEDBY") +"<br/>Waive requested Date :  " + DataBinder.Eval(Container,"DataItem.FLDREQUESTEDDATE","{0:dd/MMM/yyyy}") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Reason for</br> Waive requested" HeaderStyle-HorizontalAlign="Center">
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblReason" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.FLDWAIVINGREASON")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <%--   <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtReason" runat="server"  Text='<%# (DataBinder.Eval(Container, "DataItem.FLDWAIVINGREASON")) %>'></telerik:RadTextBox>
                                    </EditItemTemplate>--%>
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

                                <telerik:GridTemplateColumn HeaderText="Waived Y/N" HeaderStyle-HorizontalAlign="Center">
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="90px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkWaivedconfirmYN" runat="server" Enabled="false"
                                            Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDISWAIVED")).ToString().Equals("1")?true:false %>' />
                                        <eluc:ToolTip ID="ucWaiveToolTipDate" runat="server" Text='<%# "Waived by : " + DataBinder.Eval(Container,"DataItem.FLDWAIVEDUSERNAME") +"<br/>Waived Date :  " + DataBinder.Eval(Container,"DataItem.FLDWAIVEDDATE","{0:dd/MMM/yyyy}") %>' />
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
                                <telerik:GridTemplateColumn HeaderText="Expiry Date">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblExpiryDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Issuing Authority">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblNationality" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNATIONALITY") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Action">
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="90px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblwaivedocid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAIVEDDOCID") %>'></telerik:RadLabel>
                                        <asp:LinkButton runat="server" AlternateText="Approve"
                                            CommandName="APPROVE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdApprove" Visible="false"
                                            ToolTip="Waive">
                                            <span class="icon"><i class="fas fa-award"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn Visible="false">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblActionHeader" runat="server">
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        Action
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
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
                                            <span class="icon"><i class="fas fa-times-circle"></i></span>
                                        </asp:LinkButton>
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
                </div>
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
                <br />
                <br />
                <b>
                    <telerik:RadLabel ID="lblRankExpNote" runat="server" Text="Rank experience required in ANY of the below ranks"></telerik:RadLabel>
                </b>
                <div id="divRank" style="position: relative; z-index: +1">
                    <%--  <asp:GridView ID="gvRankExp" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="gvRankExp_RowDataBound" ShowHeader="true"
                        EnableViewState="true" OnRowCommand="gvRankExp_RowCommand">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvRankExp" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvRankExp_NeedDataSource"
                        OnItemCommand="gvRankExp_ItemCommand"
                        OnItemDataBound="gvRankExp_ItemDataBound"
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

                                <telerik:GridTemplateColumn HeaderText="Stage">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblStage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAGENAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Rank">
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblwaivedocid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAIVEDDOCID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblShortfall" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTFALL") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblDocumentType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblWaivedDocumentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAIVEDDOCID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblStageid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAGEID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblPlanStatus" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANSTATUS") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblProposalStageid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSALSTAGEID") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Can be  <br/>waived later" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" Visible="false">
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
                                            OnCheckedChanged="chkWaivedYN_CheckedChanged" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDISWAIVED")).ToString().Equals("0")?false:true %>' />
                                        <eluc:ToolTip ID="ucToolTipDate" runat="server" Text='<%# "Reason for Waive requested : " + DataBinder.Eval(Container,"DataItem.FLDWAIVINGREASON") +"<br/>Waive requested by : " + DataBinder.Eval(Container,"DataItem.FLDREQUESTEDBY") +"<br/>Waive requested Date :  " + DataBinder.Eval(Container,"DataItem.FLDREQUESTEDDATE","{0:dd/MMM/yyyy}") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Reason for</br> Waive requested" HeaderStyle-HorizontalAlign="Center" Visible="false">
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
                                <telerik:GridTemplateColumn HeaderText="Waived Y/N">
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="90px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkWaivedconfirmYN" runat="server" Enabled="false"
                                            Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDISWAIVED")).ToString().Equals("1")?true:false %>' />
                                        <eluc:ToolTip ID="ucWaiveToolTipDate" runat="server" Text='<%# "Waived by : " + DataBinder.Eval(Container,"DataItem.FLDWAIVEDUSERNAME") +"<br/>Waived Date :  " + DataBinder.Eval(Container,"DataItem.FLDWAIVEDDATE","{0:dd/MMM/yyyy}") %>' />
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
                                <telerik:GridTemplateColumn HeaderText="Action">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Approve"
                                            CommandName="APPROVERANK" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdApprove"
                                            ToolTip="Waive">
                                         <span class="icon"><i class="fas fa-award"></i></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" AlternateText="Edit"
                                            CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                            ToolTip="Edit" Visible="false">                                        
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
                                        <span class="icon"><i class="fas fa-times-circle"></i></span>
                                        </asp:LinkButton>
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
                </div>
                <br />
                <br />

                <b>
                    <telerik:RadLabel ID="lblVesselTypeExpNote" runat="server" Text="Vessel Type experience required in ANY of the below vessel types"></telerik:RadLabel>
                </b>
                <div id="divVT" style="position: relative; z-index: +1">
                    <%--  <asp:GridView ID="gvVesselTypeExp" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="gvVesselTypeExp_RowDataBound" ShowHeader="true"
                        EnableViewState="true" OnRowCommand="gvVesselTypeExp_RowCommand">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvVesselTypeExp" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvVesselTypeExp_NeedDataSource"
                        OnItemCommand="gvVesselTypeExp_ItemCommand"
                        OnItemDataBound="gvVesselTypeExp_ItemDataBound"
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

                                <telerik:GridTemplateColumn HeaderText="Stage">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblStage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAGENAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Vessel Type">
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblwaivedocid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAIVEDDOCID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblVesselType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEDESCRIPTION") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblShortfall" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTFALL") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblDocumentType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblWaivedDocumentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAIVEDDOCID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblStageid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAGEID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblPlanStatus" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANSTATUS") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblProposalStageid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSALSTAGEID") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Can be  <br/>waived later" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" Visible="false">
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
                                            OnCheckedChanged="chkWaivedYN_CheckedChanged" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDISWAIVED")).ToString().Equals("0")?false:true %>' />
                                        <eluc:ToolTip ID="ucToolTipDate" runat="server" Text='<%# "Reason for Waive requested : " + DataBinder.Eval(Container,"DataItem.FLDWAIVINGREASON") +"<br/>Waive requested by : " + DataBinder.Eval(Container,"DataItem.FLDREQUESTEDBY") +"<br/>Waive requested Date :  " + DataBinder.Eval(Container,"DataItem.FLDREQUESTEDDATE","{0:dd/MMM/yyyy}") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Reason for</br> Waive requested" HeaderStyle-HorizontalAlign="Center" Visible="false">
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
                                <telerik:GridTemplateColumn HeaderText="Waived Y/N">
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="90px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkWaivedconfirmYN" runat="server" Enabled="false"
                                            Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDISWAIVED")).ToString().Equals("1")?true:false %>' />
                                        <eluc:ToolTip ID="ucWaiveToolTipDate" runat="server" Text='<%# "Waived by : " + DataBinder.Eval(Container,"DataItem.FLDWAIVEDUSERNAME") +"<br/>Waived Date :  " + DataBinder.Eval(Container,"DataItem.FLDWAIVEDDATE","{0:dd/MMM/yyyy}") %>' />
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
                                <telerik:GridTemplateColumn HeaderText="Action">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                   
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Approve" 
                                            CommandName="APPROVETYPE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdApprove"
                                            ToolTip="Waive">
                                            <span class="icon"><i class="fas fa-award"></i></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" AlternateText="Edit" 
                                            CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                            ToolTip="Edit" Visible="false">
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
                                            <span class="icon"><i class="fas fa-times-circle"></i></span>
                                        </asp:LinkButton>
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
                </div>
                <br />
                <br />
                <br />
            </div>
        </telerik:RadAjaxPanel>

    </form>
</body>
</html>
