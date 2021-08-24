<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersVesselSurveyPlanConfiguration.aspx.cs"
    Inherits="Registers_RegistersVesselSurveyPlanConfiguration" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cycles</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersSurveyCycles" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MainMenuRegistersSurveyCycles" runat="server" OnTabStripCommand="MainMenuRegistersSurveyCycles_TabStripCommand"></eluc:TabStrip>
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTemplateDesc" runat="server" Text="Validity Cycle"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTemplate" runat="server" CssClass="readonlytextbox" Width="250px"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersSurveyCycles" runat="server" OnTabStripCommand="MenuRegistersSurveyCycles_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvSurveyCycles" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvSurveyCycles_NeedDataSource" Height="40%" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnItemDataBound="gvSurveyCycles_ItemDataBound"
                OnItemCommand="gvSurveyCycles_ItemCommand"
                OnUpdateCommand="gvSurveyCycles_UpdateCommand"
                OnSortCommand="gvSurveyCycles_SortCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDSEQUENCEID" ShowFooter="true">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Survey Type">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSurveyTypeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSURVEYTYPEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSurveyType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSURVEYTYPENAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblConfigurationId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEQUENCEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblTemplateId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTEMPLATEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblSurveyTypeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSURVEYTYPEID") %>'></telerik:RadLabel>
                                <telerik:RadComboBox ID="ddlSurveyTypeEdit" runat="server" DataSource='<%# PhoenixRegisterVesselSurveyConfiguration.SurveyTypeList(1) %>'
                                    DataValueField="FLDSURVEYTYPEID" DataTextField="FLDSURVEYTYPENAME" CssClass="dropdown_mandatory">
                                </telerik:RadComboBox>
                                <telerik:RadLabel ID="lblConfigurationId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEQUENCEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblTemplateId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTEMPLATEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlSurveyTypeAdd" runat="server" DataSource='<%# PhoenixRegisterVesselSurveyConfiguration.SurveyTypeList(1) %>'
                                    DataValueField="FLDSURVEYTYPEID" DataTextField="FLDSURVEYTYPENAME" CssClass="dropdown_mandatory">
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sequence">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSequence" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEQUENCE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucSequenceEdit" runat="server" CssClass="input_mandatory" Width="50px"
                                    MaxLength="2" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEQUENCE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucSequenceAdd" runat="server" CssClass="input_mandatory" Width="50px"
                                    MaxLength="1" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Months to add for Next Due">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNextDue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEXTDUE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucNextDueEdit" CssClass="input_mandatory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEXTDUE") %>'
                                    Width="90%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucNextDueAdd" CssClass="input_mandatory" runat="server" Width="90%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Next Survey Type">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNextSurveyTypeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEXTSURVEYTYPEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblNextSurveyType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEXTSURVEYTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblNextSurveyTypeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEXTSURVEYTYPEID") %>'></telerik:RadLabel>
                                <telerik:RadComboBox ID="ddlNextSurveyTypeEdit" runat="server" DataSource='<%# PhoenixRegisterVesselSurveyConfiguration.SurveyTypeList(1) %>'
                                    DataValueField="FLDSURVEYTYPEID" DataTextField="FLDSURVEYTYPENAME" CssClass="dropdown_mandatory">
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlNextSurveyTypeAdd" runat="server" DataSource='<%# PhoenixRegisterVesselSurveyConfiguration.SurveyTypeList(1) %>'
                                    DataValueField="FLDSURVEYTYPEID" DataTextField="FLDSURVEYTYPENAME" CssClass="dropdown_mandatory">
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Next Sequence">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDNEXTSEQUENCE") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucNextSequenceEdit" runat="server" CssClass="input_mandatory" Width="50px"
                                    MaxLength="2" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEXTSEQUENCE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucNextSequenceAdd" runat="server" CssClass="input_mandatory" Width="50px"
                                    MaxLength="2" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" UniqueName="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                        <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" CommandName="Update" ID="cmdUpdate" ToolTip="Update">
                                <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                <span class="icon"><i class="fas fa-times"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
                                <span class="icon"><i class="fas fa-plus-square"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="5" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status runat="server" ID="ucStatus" />

            <br />
            <br />
            <div>
                <table width="25%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblSurveyChange" runat="server" Text="Survey Change Required in "></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlSurveyChange" runat="server" Width="100px" CssClass="input" AppendDataBoundItems="true">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                </table>
            </div>
            <b>
                <asp:Literal ID="lblSurveyChangeCycle" Visible="false" runat="server" Text="Survey Cycle Change"></asp:Literal></b>
            <eluc:TabStrip ID="MenuSCExcel" runat="server" OnTabStripCommand="MenuSCExcel_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvSurveyChange" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvSurveyChange_NeedDataSource" Height="40%" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnItemDataBound="gvSurveyChange_ItemDataBound"
                OnUpdateCommand="gvSurveyChange_UpdateCommand"
                OnSortCommand="gvSurveyChange_SortCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDSURVEYTYPEID" ShowFooter="true">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Survey Type">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSurveyChangeSequenceId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEQUENCEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSurveyChangeTypeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSURVEYTYPEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSurveyChangeTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSURVEYTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sequence">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSurveyChangeSequenceNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEQUENCE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Months to add for Next Due Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSurveyChangeNextDueMonth" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEXTDUE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucSurveyChangeNextDueEdit" CssClass="input_mandatory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEXTDUE") %>'
                                    Width="90%" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Next Survey Type">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSurveyChangeNextSurveyId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEXTSURVEYTYPEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSurveyChangeNextSurveyName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEXTSURVEYTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Next Sequence">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSurveyChangeNextSequenceNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEXTSEQUENCE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" CommandName="Update" ID="cmdUpdate" ToolTip="Update">
                                <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                <span class="icon"><i class="fas fa-times"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="5" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
