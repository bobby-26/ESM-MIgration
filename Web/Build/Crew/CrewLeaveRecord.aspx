<%@ Page Language="C#" AutoEventWireup="True" CodeFile="CrewLeaveRecord.aspx.cs" Inherits="CrewLeaveRecord" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rank</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="frmRegistersRankk" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
    <form id="frmLeaveRecord" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="92%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <telerik:RadButton runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuLeaveRecord" runat="server" OnTabStripCommand="MenuLeaveRecord_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvLVR" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvLVR_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                EnableViewState="false" EnableHeaderContextMenu="true" Width="100%" Height="97%"
                OnItemDataBound="gvLVR_ItemDataBound" OnItemCommand="gvLVR_ItemCommand"
                ShowFooter="false" ShowHeader="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDDTKEY" TableLayout="Fixed" CommandItemDisplay="Top">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Crew Name">
                            <HeaderStyle Width="170px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDEMPLOYEENAME")%>
                                <telerik:RadLabel ID="lbldtkey" runat="server" Visible="false" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDDTKEY")%>'></telerik:RadLabel>

                            </ItemTemplate>
                            <FooterTemplate>
                                <span id="spnPickListEmployeeAdd" runat="server">
                                    <telerik:RadTextBox ID="txtEmployeeNameAdd" runat="server"
                                        CssClass="input_mandatory" Width="80%">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowEmployeeAdd" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" />
                                    <telerik:RadTextBox ID="txtEmployeeIdAdd" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                </span>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <HeaderStyle Width="160px" />
                            <HeaderStyle Width="115px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDRANKNAME")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Rank ID="ddlRankEdit" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" RankList="<%#PhoenixRegistersRank.ListRank() %>"
                                    SelectedRank='<%# DataBinder.Eval(Container, "DataItem.FLDRANKID")%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Rank ID="ddlRankAdd" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                    RankList="<%#PhoenixRegistersRank.ListRank() %>" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel">
                            <HeaderStyle Width="150px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDVESSELNAME")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Vessel ID="ddlVesselEdit" runat="server" VesselsOnly="true" AppendDataBoundItems="true" VesselList="<%#PhoenixRegistersVessel.ListVessel() %>" SelectedVessel='<%# DataBinder.Eval(Container, "DataItem.FLDVESSELID")%>'
                                    CssClass="input_mandatory" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Vessel ID="ddlVesselAdd" runat="server" VesselsOnly="true" AppendDataBoundItems="true" CssClass="input_mandatory"
                                    VesselList="<%#PhoenixRegistersVessel.ListVessel() %>" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="From">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# string.Format("{0:dd/MMM/yyyy}", DataBinder.Eval(Container, "DataItem.FLDFROMDATE"))%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="txtFromDateEdit" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFROMDATE")%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date ID="txtFromDateAdd" runat="server" CssClass="input_mandatory" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="To">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# string.Format("{0:dd/MMM/yyyy}", DataBinder.Eval(Container, "DataItem.FLDTODATE")) %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="txtToDateEdit" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTODATE")%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date ID="txtToDateAdd" runat="server" CssClass="input_mandatory" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Leave Earned">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDLEAVEEARNED")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number runat="server" ID="txtLeaveEarnedEdit" CssClass="input_mandatory" DecimalPlace="1" IsPositive="true"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDLEAVEEARNED")%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number runat="server" ID="txtLeaveEarnedAdd" CssClass="input_mandatory" DecimalPlace="1" IsPositive="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Leave UnPaid">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDLEAVEUNPAID")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number runat="server" ID="txtLeaveUnPadidEdit" CssClass="input_mandatory" DecimalPlace="1" IsPositive="true"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDLEAVEUNPAID")%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number runat="server" ID="txtLeaveUnPadidAdd" CssClass="input_mandatory" DecimalPlace="1" IsPositive="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="BTB Earned">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDBTBEARNED")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number runat="server" ID="txtBTBEarnedEdit" CssClass="input_mandatory" DecimalPlace="1" IsPositive="true"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDBTBEARNED")%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number runat="server" ID="txtBTBEarnedAdd" CssClass="input_mandatory" DecimalPlace="1" IsPositive="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="BTB UnPaid">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDBTBUNPAID")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number runat="server" ID="txtBTBUnPaidEdit" CssClass="input_mandatory" DecimalPlace="1" IsPositive="true"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDBTBUNPAID")%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number runat="server" ID="txtBTBUnPaidAdd" CssClass="input_mandatory" DecimalPlace="1" IsPositive="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="7%"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>

                                <asp:LinkButton runat="server" AlternateText="Transfer" CommandName="SELECT" ID="cmdTransfer" ToolTip="BTB Transfer">
                                    <span class="icon"><i class="fas fa-check"></i></span>
                                </asp:LinkButton>

                                <%--  <asp:ImageButton runat="server" AlternateText="Transfer" ImageUrl="<%$ PhoenixTheme:images/61.png %>"
                                    CommandName="SELECT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdTransfer"
                                    ToolTip="BTB Transfer"></asp:ImageButton>--%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>


                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="ADD" ID="cmdAdd" ToolTip="Add New">
                                    <span class="icon"><i class="fas fa-check-circle"></i></span>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                    EnablePostBackOnRowClick="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="2" EnableNextPrevFrozenColumns="true" ScrollHeight="415px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>

            </telerik:RadGrid>
        </telerik:RadAjaxPanel>

    </form>
</body>
</html>

