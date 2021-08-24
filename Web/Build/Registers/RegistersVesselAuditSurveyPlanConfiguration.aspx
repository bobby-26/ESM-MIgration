<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersVesselAuditSurveyPlanConfiguration.aspx.cs"
    Inherits="RegistersVesselAuditSurveyPlanConfiguration" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Audit Survey Cycles</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersSurveyCycles" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
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
            CellSpacing="0" GridLines="None" OnNeedDataSource="gvSurveyCycles_NeedDataSource" Height="80%" GroupingEnabled="false" EnableHeaderContextMenu="true"
            OnItemDataBound="gvSurveyCycles_ItemDataBound"
            OnItemCommand="gvSurveyCycles_ItemCommand"
            OnUpdateCommand="gvSurveyCycles_UpdateCommand"
            OnSortCommand="gvSurveyCycles_SortCommand">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDAUDITID" ShowFooter="true">
                <HeaderStyle Width="102px" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Audit">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAuditId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAUDITID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblAuditName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAUDITNAME") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblConfigurationId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEQUENCEID") %>'
                                Visible="false">
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="lblTemplateId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTEMPLATEID") %>'
                                Visible="false">
                            </telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadLabel ID="lblAuditId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAUDITID") %>'></telerik:RadLabel>
                            <telerik:RadComboBox ID="ddlAuditEdit" runat="server" DataValueField="FLDINSPECTIONID" DataTextField="FLDSHORTCODE">
                            </telerik:RadComboBox>
                            <telerik:RadLabel ID="lblConfigurationId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEQUENCEID") %>'
                                Visible="false">
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="lblTemplateId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTEMPLATEID") %>'
                                Visible="false">
                            </telerik:RadLabel>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <telerik:RadComboBox ID="ddlAuditAdd" runat="server"
                                DataValueField="FLDINSPECTIONID" DataTextField="FLDSHORTCODE" CssClass="dropdown_mandatory">
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
                                MaxLength="1" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEQUENCE") %>' />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <eluc:Number ID="ucSequenceAdd" runat="server" CssClass="input_mandatory" Width="50px"
                                MaxLength="1" />
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Months to add for Next Due Date">
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
                    <telerik:GridTemplateColumn HeaderText="Next Audit">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblNextAuditId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEXTAUDITID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblNextAudit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEXTAUDITNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadLabel ID="lblNextAuditId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEXTAUDITID") %>'></telerik:RadLabel>
                            <telerik:RadComboBox ID="ddlNextAuditEdit" runat="server" DataValueField="FLDINSPECTIONID" DataTextField="FLDSHORTCODE">
                            </telerik:RadComboBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <telerik:RadComboBox ID="ddlNextAuditAdd" runat="server"
                                DataValueField="FLDINSPECTIONID" DataTextField="FLDSHORTCODE" CssClass="dropdown_mandatory">
                            </telerik:RadComboBox>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn>
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
                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="center">
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
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
    </form>
</body>
</html>

