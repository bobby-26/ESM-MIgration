<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionIncidentInjuryList.aspx.cs" Inherits="InspectionIncidentInjuryList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hazard" Src="~/UserControls/UserControlRAHazardType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Incident Damage List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvIncidentInjury.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
                fade('statusmessage');
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmIncidentInjury" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuIncidentInjuryGeneral" runat="server" OnTabStripCommand="MenuIncidentInjuryGeneral_TabStripCommand" Title="Health and Safety"></eluc:TabStrip>
            <table id="tblConfigureIncidentInjury" width="100%">
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblThirdPartyInjury" runat="server" Text="Third Party Injury"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadCheckBox ID="chkThirdPartyInjury" runat="server" AutoPostBack="true" OnCheckedChanged="ThirdParty_Changed" />
                    </td>
                    <%--<td colspan="2"></td>--%>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblAge" runat="server" Text="Age"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtAge" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            MaxLength="20" Width="20%">
                        </telerik:RadTextBox>
                        <eluc:Number ID="txtThirdPartyAge" runat="server" CssClass="input_mandatory txtNumber"
                            MaxLength="20" Width="20%" IsPositive="true" Visible="false" IsInteger="true" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblInjuredName" runat="server" Text="Injured's Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <span id="spnCrewInCharge" runat="server">
                            <telerik:RadTextBox ID="txtCrewName" runat="server" CssClass="input_mandatory" Enabled="false"
                                MaxLength="50" Width="50%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtCrewRank" runat="server" CssClass="input_mandatory" Enabled="false"
                                MaxLength="50" Width="30%">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="imgShowCrewInCharge">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtCrewId" runat="server" CssClass="hidden" MaxLength="20" Width="10px"></telerik:RadTextBox>
                        </span>
                        <span id="spnPersonInChargeOffice" runat="server">
                            <telerik:RadTextBox ID="txtOfficePersonName" runat="server" CssClass="input_mandatory" Enabled="false"
                                MaxLength="50" Width="50%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtOfficePersonDesignation" runat="server" CssClass="input_mandatory" Enabled="false"
                                MaxLength="50" Width="23%">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="imgPersonOffice">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox runat="server" ID="txtPersonOfficeId" CssClass="hidden" Width="0px"
                                MaxLength="20">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox runat="server" ID="txtPersonOfficeEmail" CssClass="hidden" Width="0px"
                                MaxLength="20">
                            </telerik:RadTextBox>
                        </span>
                        <span id="spnThirdParty" runat="server" visible="false">
                            <telerik:RadTextBox ID="txtThirdPartyName" runat="server" CssClass="input_mandatory" MaxLength="50"
                                Width="50%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtThirdPartyDesignation" runat="server" CssClass="input_mandatory"
                                MaxLength="50" Width="30%">
                            </telerik:RadTextBox>
                        </span>
                        <telerik:RadLabel ID="lblDtkey" runat="server" Visible="false"></telerik:RadLabel>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblServiceYearsatSea" runat="server" Text="Service Years at Sea"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtServiceYearsAtSea" runat="server" CssClass="readonlytextbox"
                            MaxLength="20" ReadOnly="true" Width="20%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblServiceYearsinCompany" runat="Server" Text="Service Years in Company"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtServiceYears" runat="server" CssClass="readonlytextbox" MaxLength="20"
                            ReadOnly="true" Width="20%">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblPartoftheBodyInjured" runat="server" Text="Part of the Body Injured"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadComboBox ID="ddlPartofTheBodyInjured" runat="server" AutoPostBack="true" AppendDataBoundItems="true"
                            EmptyMessage="Type to select " Filter="Contains" CssClass="input_mandatory" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                            Width="240px">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblTypeofInjury" runat="server" Text="Type of Injury"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Quick ID="ucTypeOfInjury" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                            QuickTypeCode="69" Width="240px" />
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblWorkDaysLost" runat="server" Text="Work days lost"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Number ID="ucManHoursLost" runat="server" CssClass="input txtNumber" IsInteger="true"
                            IsPositive="true" MaxLength="8" Width="20%" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblCategoryofWorkInjury" runat="server" Text="Category of Work Injury"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadComboBox ID="ddlWorkInjuryCategory" runat="server" Width="240px" CssClass="input_mandatory" Filter="Contains">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblHealthandSafetySubcategory" runat="server" Text="Health and Safety Subcategory"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlHealthSafetySubCategory" Width="240px" AppendDataBoundItems="true"
                            runat="server" CssClass="input_mandatory" DataTextField="FLDNAME" DataValueField="FLDSUBHAZARDID"
                            Filter="Contains">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblHealthandSafetyCategory" runat="server" Text="Health and Safety Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hazard ID="ucHealthSafetyCategory" Type="1" runat="server" AutoPostBack="true"
                            Width="240px" AppendDataBoundItems="true" OnTextChangedEvent="ucHealthSafetyCategory_Changed"
                            CssClass="input_mandatory" />
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblEstimatedCostinUSD" runat="server" Text="Estimated Cost in USD"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Number ID="ucExtimatedCost" runat="server" CssClass="input txtNumber" DecimalPlace="2"
                            MaxLength="10" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox runat="server" ID="txtRemarks" CssClass="input" TextMode="MultiLine"
                            Height="70px" Width="81%" Resize="Both">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox runat="server" ID="txtDescription" CssClass="input" TextMode="MultiLine"
                            Height="70px" Width="80%" Visible="false" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblConsequencePotentialCategory" runat="server" Text="Consequence/Potential Category"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtCategory" runat="server" Enabled="false" Width="30px" ReadOnly="true"
                            CssClass="input">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:LinkButton ID="lnkSicknessReport" runat="server" Text="CR11 Sickness Report"></asp:LinkButton>
                    </td>
                    <td colspan="2"></td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuGeneral" runat="server" OnTabStripCommand="MenuGeneral_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvIncidentInjury" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowCustomPaging="true" AllowPaging="true"
                Width="100%" CellPadding="3" OnItemCommand="gvIncidentInjury_ItemCommand" OnItemDataBound="gvIncidentInjury_ItemDataBound"
                AllowSorting="true" ShowFooter="false" ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvIncidentInjury_NeedDataSource"
                EnableHeaderContextMenu="true" GroupingEnabled="false" GridLines="None" CellSpacing="0">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDINSPECTIONINCIDENTINJURYID">
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Injured's Name">
                            <HeaderStyle Width="12%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIncidentInjuryId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONINCIDENTINJURYID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblMedicalCaseId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEDICALCASEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIncidentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONINCIDENTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkCrewName" runat="server" CommandName="SELECT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Age">
                            <HeaderStyle Width="5%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAge" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGE" ) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Designation">
                            <HeaderStyle Width="10%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME" ) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Part of Body Injured">
                            <HeaderStyle Width="15%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPartOfBodyInjured" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARTNAMEOFTHEBODYINJURED" ) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type of Injury">
                            <HeaderStyle Width="13%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTypeOfDamage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFINJURYNAME" ) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category of Work Injury">
                            <HeaderStyle Width="15%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategoryOfWorkInjury" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAMEOFWORKINJURY" ) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Work Days Lost">
                            <HeaderStyle Width="10%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblManHoursLost" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANHOURSLOST" ) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Estimated Cost in USD">
                            <HeaderStyle Width="10%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEstimatedCost" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDESTIMATEDCOST" ) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Consequence Potential Category">
                            <HeaderStyle Width="10%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORY" ) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit Incident" Visible="false">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete Incident">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attachment" CommandName="ATTACHMENT" ID="cmdAtt" ToolTip="Attach Sickness Report">
                                    <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
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
