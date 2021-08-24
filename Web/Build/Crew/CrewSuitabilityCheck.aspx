<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewSuitabilityCheck.aspx.cs"
    Inherits="CrewSuitabilityCheck" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PhoneNumber" Src="~/UserControls/UserControlPhoneNumber.ascx" %>
<%--<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>--%>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlCommonVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Suitability Check</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>

    <style type="text/css">
        .hidden {
            display: none;
        }

        .rgGroupCol {
            padding-left: 0 !important;
            padding-right: 0 !important;
            font-size: 1px !important;
        }

        .rgExpand,
        .rgCollapse {
            display: none !important;
        }
    </style>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmCrewSuitabilityCheck" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="frmCrewSuitabilityCheck" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server">
        </telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="90%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <table runat="server" id="tblPersonalMaster" width="100%">
                <tr>
                    <td style="width: 13%;">
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 20%;">
                        <asp:TextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" Enabled="false" ReadOnly="true" Width="200px"></asp:TextBox>
                    </td>
                    <td style="width: 13%;">
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 20%;">
                        <asp:TextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" Enabled="false" ReadOnly="true" Width="200px"></asp:TextBox>
                    </td>
                    <td style="width: 13%;">
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 20%;">
                        <asp:TextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" Enabled="false" ReadOnly="true" Width="200px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td runat="server" id="tdempno">
                        <telerik:RadLabel ID="lblEmployeeNo" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td runat="server" id="tdempnum">
                        <asp:TextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" Enabled="false" ReadOnly="true" Width="200px"></asp:TextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <asp:TextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true" Enabled="false" Width="200px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel runat="server" ID="ucVessel" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                            VesselsOnly="true" AutoPostBack="true" OnTextChangedEvent="SetVesselType" EntityType="VSL" ActiveVessels="true" Width="200px" />

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true" CssClass="readonlytextbox"
                            Enabled="false" Width="200px" />
                    </td>
                    <td rowspan="2" colspan="2">
                        <telerik:RadLabel ID="lblvessellist" runat="server" CssClass="guideline_text">
                            Note: The Seafarer is not compatible to following vessel types.
                        </telerik:RadLabel>
                        <br />
                        <asp:TextBox ID="txtInCompVslType" runat="server" CssClass="readonlytextbox" Width="300px"
                            Height="40px" TextMode="MultiLine" ReadOnly="true" Enabled="false"></asp:TextBox></td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRank1" runat="server" Text=" Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox DropDownPosition="Static" CssClass="input" ID="ddlRank" runat="server" EnableLoadOnDemand="True"
                            EmptyMessage="Type to select Rank" Filter="Contains" MarkFirstMatch="true" DataTextField="FLDRANKNAME" DataValueField="FLDRANKID" Width="200px">
                        </telerik:RadComboBox>

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblExpectedJoiningDate" runat="server" Text=" Expected Joining Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucDate" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td colspan="4"></td>
                </tr>
            </table>
            <telerik:RadLabel ID="lblNote" runat="server" CssClass="guideline_text">
                Note: Please note validity of document checked for "contract +3 months" starting from the expected joining date.
            </telerik:RadLabel>

            <eluc:TabStrip ID="MenuCrewSuitabilityList" runat="server" OnTabStripCommand="CrewSuitabilityList_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvSuitability" Height="62%" runat="server" AllowSorting="true" AllowPaging="false"
                CellSpacing="0" GridLines="None" OnItemDataBound="gvSuitability_ItemDataBound" OnPreRender="gvSuitability_PreRender" EnableHeaderContextMenu="true"
                ShowFooter="false" ShowHeader="true" EnableViewState="true" OnNeedDataSource="gvSuitability_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                    AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
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
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn Visible="false" UniqueName="Category" DataField="FLDCATEGORY">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Document">
                            <HeaderStyle Width="30%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDocumentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblMissingYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMISSINGYN") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblExpiredYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIREDYN") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblGoExpiredYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGOEXPIREDYN") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFlagId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLAGID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDocumentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expiry Date">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExpiryDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY", "{0:dd/MM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Nationality">
                            <HeaderStyle Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNationality" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNATIONALITY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Alternate Rank">
                            <HeaderStyle Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAlternateRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALTERNATERANK") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAlternatDocGoExp" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALTERNATEDOCGOEXPYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Attachment"
                                    CommandName="Attachment" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAtt"
                                    ToolTip="Attachment"><span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td style="width: 5px; background-color: Red"></td>
                    <td>* Documents Expired
                    </td>
                    <td style="width: 5px; background-color: Blue"></td>
                    <td>* Documents Missing
                    </td>
                    <td style="width: 5px; background-color: DarkOrange"></td>
                    <td>* Documents Expired Before contract + 3 months.
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
