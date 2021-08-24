<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnerBudgetProposedCrewWages.aspx.cs"
    Inherits="OwnerBudgetProposedCrewWages" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlGroupRank.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Proposed Crew Wages</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvCrewWages.ClientID %>"));
                }, 200);
            }
        </script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersCountry" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="ToolkitScriptManager1"
            runat="server">
        </telerik:RadScriptManager>
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel runat="server" ID="pnlCrewWages">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuCrewWages" runat="server" TabStrip="true" OnTabStripCommand="MenuCrewWages_TabStripCommand" />

            <eluc:TabStrip ID="MenuShowCrewWages" runat="server" OnTabStripCommand="MenuShowCrewWages_TabStripCommand" />


            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewWages" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                OnRowCreated="gvCrewWages_RowCreated" Width="100%" CellPadding="3" OnItemCommand="gvCrewWages_ItemCommand" GroupingEnabled="false" EnableHeaderContextMenu="true"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                OnItemDataBound="gvCrewWages_ItemDataBound" OnNeedDataSource="gvCrewWages_NeedDataSource"
                OnDeleteCommand="gvCrewWages_DeleteCommand" OnUpdateCommand="gvCrewWages_UpdateCommand"
                ShowFooter="true" ShowHeader="true" EnableViewState="false"
                OnSortCommand="gvCrewWages_SortCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" CommandItemDisplay="Top">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <CommandItemSettings ShowRefreshButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <GroupHeaderTemplate>
                    </GroupHeaderTemplate>

                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Crew" HeaderStyle-Width="155px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRankid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCrew" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPRANK") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <eluc:Rank ID="ddlRankAdd" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                                    RankList="<%#PhoenixRegistersLevelRankList.ListGroupRank() %>" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Owner Scale" HeaderStyle-Width="115px" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNos" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERSCALE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucOwnerScaleEdit" CssClass="input_mandatory" DecimalPlace="0" IsInteger="true"
                                    runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERSCALE") %>' />
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <eluc:Number ID="ucOwnerScaleAdd" CssClass="input_mandatory" DecimalPlace="0" IsInteger="true"
                                    runat="server" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn FooterText="Current Wages" HeaderText="Current Wages Per Month" HeaderStyle-Width="170px" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCWPerMonth" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTWAGE","{0:0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucCurrentWagePerMonth" CssClass="input_mandatory" DecimalPlace="2"
                                    runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTWAGE","{0:0.00}") %>' />
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Right" />
                            <FooterTemplate>
                                <eluc:Number ID="ucCurrentWagePerMonthAdd" CssClass="input_mandatory" DecimalPlace="2"
                                    runat="server" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Current Wages Per Day" HeaderStyle-Width="155px" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCWPerDay" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTWAGEPERDAY","{0:0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn FooterText="Proposed Wages" HeaderText="Proposed Wages Per Month" HeaderStyle-Width="195px" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPWPerMonth" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSEDWAGE","{0:0.00}")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucProposedWagePerMonth" CssClass="input_mandatory" DecimalPlace="2"
                                    runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSEDWAGE") %>' />
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Right" />
                            <FooterTemplate>
                                <eluc:Number ID="ucProposedWagePerMonthAdd" CssClass="input_mandatory" DecimalPlace="2"
                                    runat="server" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Proposed Wages Per Day" HeaderStyle-Width="170px" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPWPerDay" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSEDWAGEPERDAY","{0:0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total Current Wages Per Month" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTotalCWPerMonth" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALCURRENTWAGE","{0:0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total Proposed Wages Per Month" HeaderStyle-Width="210px" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTotalPWPerMonth" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALPROPOSEDWAGE","{0:0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                            <HeaderStyle Width="70px" />
                            <ItemStyle Width="20px" Wrap="false" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add" CommandName="Add" ID="cmdAdd"
                                    ToolTip="Add New"> <span class="icon"><i class="fas fa-plus-circle"></i> </span>  </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true"/>
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br />
            <br />
            <br />
            <span id="span1" runat="server" title="View" style="display: inline-block; margin-left: 20px; width: 780px;"></span>
            <telerik:RadCodeBlock runat="server">
                <script type="text/javascript">
                    Sys.Application.add_load(function () {
                        setTimeout(function () {
                            TelerikGridResize($find("<%= gvCrewWages.ClientID %>"));
                    }, 200);
                });
                </script>
            </telerik:RadCodeBlock>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
