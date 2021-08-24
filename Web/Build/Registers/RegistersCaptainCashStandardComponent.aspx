<%@ Page Language="C#" AutoEventWireup="True" CodeFile="RegistersCaptainCashStandardComponent.aspx.cs"
    Inherits="RegistersCaptainCashStandardComponent" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Captain Cash Standard Component</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
    <form id="frmRegistersRank" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuCCStandardComponent" runat="server" OnTabStripCommand="CCStandardComponent_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCCStdComp" Height="97%" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" OnItemCommand="gvCCStdComp_ItemCommand" OnItemDataBound="gvCCStdComp_ItemDataBound" EnableViewState="false"
                ShowFooter="true" ShowHeader="true" OnNeedDataSource="gvCCStdComp_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDDTKEY">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Component Type">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <%# ((DataRowView)Container.DataItem)["FLDLOGTYPENAME"]%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lbldtkeyedit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadComboBox DropDownPosition="Static" ID="ddlComponentTypeEdit" runat="server" EnableLoadOnDemand="True"
                                    EmptyMessage="Type to select Type" Filter="Contains" MarkFirstMatch="true" CssClass="input_mandatory" Width="95%"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlComponentType_SelectedIndexChanged" DataTextField="FLDNAME" DataValueField="FLDCOMPONENTID">
                                </telerik:RadComboBox>

                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox DropDownPosition="Static" ID="ddlComponentTypeAdd" runat="server" EnableLoadOnDemand="True"
                                    EmptyMessage="Type to select Type" Filter="Contains" MarkFirstMatch="true" CssClass="input_mandatory" Width="95%"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlComponentType1_SelectedIndexChanged" DataTextField="FLDNAME" DataValueField="FLDCOMPONENTID">
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Component Sub Type">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDHARDNAME"]%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Hard ID="ddlHardEdit" runat="server" CssClass="input" AppendDataBoundItems="true"
                                    HardTypeCode="229" ShortNameFilter="AIB,AIC,AIH,AIO,AIP,AIS" HardList='<%#PhoenixRegistersHard.ListHard(1, 229, 0, "AIB,AIC,AIH,AIO,AIP,AIS")%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Hard ID="ddlHardAdd" runat="server" CssClass="input" AppendDataBoundItems="true"
                                    HardTypeCode="229" ShortNameFilter="AIB,AIC,AIH,AIO,AIP,AIS" HardList='<%#PhoenixRegistersHard.ListHard(1, 229, 0, "AIB,AIC,AIH,AIO,AIP,AIS")%>' />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Code">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDCODE"]%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtCodeEdit" runat="server" CssClass="gridinput" MaxLength="5" Text='<%# ((DataRowView)Container.DataItem)["FLDCODE"]%>'></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtCodeAdd" runat="server" CssClass="gridinput" MaxLength="5"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Description">
                            <HeaderStyle Width="25%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDDESCRIPTION"]%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtDescEdit" runat="server" CssClass="gridinput_mandatory" MaxLength="200"
                                    Text='<%# ((DataRowView)Container.DataItem)["FLDDESCRIPTION"]%>'>
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtDescAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Budget Code">
                            <HeaderStyle Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDBUDGETCODE"]%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListBudgetEdit">
                                    <telerik:RadTextBox ID="txtBudgetCodeEdit" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'
                                        MaxLength="20" CssClass="input_mandatory" Width="80%" Enabled="false">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetNameEdit" runat="server" Width="0px" CssClass="input hidden"
                                        Enabled="False">
                                    </telerik:RadTextBox>
                                    <asp:LinkButton runat="server" ID="btnShowBudgetEdit" ToolTip="Show Course" ForeColor="#4c93cc">
                                    <span class="icon"><i class="fas fas fa-list-alt"></i></span>
                                    </asp:LinkButton>

                                    <telerik:RadTextBox ID="txtBudgetIdEdit" runat="server" Width="0px" CssClass="input hidden"
                                        Text='<%#DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'>
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetgroupIdEdit" runat="server" Width="0px" CssClass="input hidden"></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <span id="spnPickListBudgetAdd">
                                    <telerik:RadTextBox ID="txtBudgetCodeAdd" runat="server" MaxLength="20" CssClass="input_mandatory"
                                        Width="80%" Enabled="false">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetNameAdd" runat="server" Width="0px" CssClass="input hidden"
                                        Enabled="False">
                                    </telerik:RadTextBox>
                                    <asp:LinkButton runat="server" ID="btnShowBudgetAdd" ToolTip="Show Course" ForeColor="#4c93cc">
                                    <span class="icon"><i class="fas fas fa-list-alt"></i></span>
                                    </asp:LinkButton>

                                    <telerik:RadTextBox ID="txtBudgetIdAdd" runat="server" Width="0px" CssClass="input hidden"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetgroupIdAdd" runat="server" Width="0px" CssClass="input hidden"></telerik:RadTextBox>
                                </span>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Save" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
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

