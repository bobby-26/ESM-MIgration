<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersNoonReportRangeConfigField.aspx.cs"
    Inherits="RegistersNoonReportRangeConfigField" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Noon Report Range Config</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style>
            .customIconAlert {
                width: 20px;
                -webkit-width: 20px;
                -moz-width: 20px;
                -ms-width: 20px;
                height: 20px;
                -ms-height: 20px;
                -webkit-height: 20px;
                -moz-height: 20px;
                cursor: pointer;
                white-space: nowrap;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmNoonReportRangeConfig" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="Status1" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuNewSaveTabStrip" TabStrip="true" runat="server" OnTabStripCommand="NewSaveTap_TabStripCommand"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuNRRangeConfig" runat="server" OnTabStripCommand="MenuNRRangeConfig_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvNRConfig" Height="88%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvNRConfig_ItemCommand" OnItemDataBound="gvNRConfig_ItemDataBound"
                OnUpdateCommand="gvNRConfig_UpdateCommand" OnNeedDataSource="gvNRConfig_NeedDataSource" OnSortCommand="gvNRConfig_SortCommand"
                ShowHeader="true" EnableViewState="true" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDNOONREPORTRANGECONFIGFIELDID">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Field Name" AllowSorting="true" SortExpression="FLDCOLUMNDESCRIPTION">
                            <HeaderStyle Width="15%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblConfigId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOONREPORTRANGECONFIGFIELDID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblConfigIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOONREPORTRANGECONFIGFIELDID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkFieldName" runat="server" Visible="false" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOLUMNDESCRIPTION") %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblFieldName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOLUMNDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <%--<EditItemTemplate>
                                <telerik:RadLabel ID="lblConfigIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOONREPORTRANGECONFIGFIELDID") %>'></telerik:RadLabel>
                                <telerik:RadDropDownList runat="server" ID="ddlFieldNameEdit" AppendDataBoundItems="true"
                                    CssClass="dropdown_mandatory" Width="100%">
                                    <Items>
                                        <telerik:DropDownListItem Text="--Select--" Value="" />
                                    </Items>
                                </telerik:RadDropDownList>
                            </EditItemTemplate>--%>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Min Value">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMinValue" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMINVALUE") %>'></telerik:RadLabel>
                                 <eluc:Number ID="txtMinValueEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMINVALUE") %>'
                                    CssClass="input" MaxLength="5" Width="100%" />
                            </ItemTemplate>
                            <%--<EditItemTemplate>
                                <eluc:Number ID="txtMinValueEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMINVALUE") %>'
                                    CssClass="input" MaxLength="5" Width="100%" />
                            </EditItemTemplate>--%>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Max Value">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMaxValue" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAXVALUE") %>'></telerik:RadLabel>
                                <eluc:Number ID="txtMaxValueEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMAXVALUE") %>'
                                    CssClass="input" MaxLength="5" Width="100%" />
                            </ItemTemplate>
                           <%-- <EditItemTemplate>
                                <eluc:Number ID="txtMaxValueEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMAXVALUE") %>'
                                    CssClass="input" MaxLength="5" Width="100%" />
                            </EditItemTemplate>--%>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Min Alert Level">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAlertLevel" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMINALLERTLEVEL") %>'></telerik:RadLabel>
                                <eluc:Number ID="txtAlertLevelEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMINALLERTLEVEL") %>'
                                    CssClass="input" MaxLength="5" Width="100%" />
                            </ItemTemplate>
                            <%--<EditItemTemplate>
                                <eluc:Number ID="txtAlertLevelEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMINALLERTLEVEL") %>'
                                    CssClass="input" MaxLength="5" Width="100%" />
                            </EditItemTemplate>--%>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Max Alert Level">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMaxAlertLevel" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAXALLERTLEVEL") %>'></telerik:RadLabel>
                                <eluc:Number ID="txtMaxAlertLevelEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMAXALLERTLEVEL") %>'
                                    CssClass="input" MaxLength="5" Width="100%" />
                            </ItemTemplate>
                            <%--<EditItemTemplate>
                                <eluc:Number ID="txtMaxAlertLevelEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMAXALLERTLEVEL") %>'
                                    CssClass="input" MaxLength="5" Width="100%" />
                            </EditItemTemplate>--%>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Req. in VPS">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="false" HorizontalAlign="center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="reqinvps" runat="server" Enabled="false" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDREQUIREDINVPS").ToString() == "1" ? true : false %>'></telerik:RadCheckBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Req. in LOG">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="false" HorizontalAlign="center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="reqinlog" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDREQUIREDINLOG").ToString() == "1" ? true : false %>'></telerik:RadCheckBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Exception Vessels">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="false" HorizontalAlign="left"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselList" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVESSELNAMELIST").ToString().Length > 50 ? DataBinder.Eval(Container, "DataItem.FLDVESSELNAMELIST").ToString().Substring(0, 50) + "..." : DataBinder.Eval(Container, "DataItem.FLDVESSELNAMELIST").ToString()%>'> </telerik:RadLabel>
                                <eluc:ToolTip ID="uclblVesselList" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAMELIST") %>' TargetControlId="lblVesselList" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" Visible="false" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Vessel List" CommandName="VESSEL" ID="cmdVessel" ToolTip="Vessel List">
                                    <span class="icon"><i class="fab fa-docker"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Color" ID="cmdPossibleCause" ToolTip="Possible Cause" CommandName="INFO" CssClass="customIconAlert">
                                    <span class="icon"><i class="fas fa-info-circle"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave"
                                    ToolTip="Save">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                            <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
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
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
