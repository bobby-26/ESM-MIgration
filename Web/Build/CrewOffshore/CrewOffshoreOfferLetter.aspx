<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreOfferLetter.aspx.cs" Inherits="CrewOffshoreOfferLetter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BankAccount" Src="~/UserControls/UserControlEmployeeBankAccount.ascx" %>
<%@ Register Src="~/UserControls/UserControlHard.ascx" TagName="Hard" TagPrefix="eluc" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Offer Letter</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmCrewAppointmentLetter" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%; position: absolute;">

                <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />


                <eluc:TabStrip ID="CrewMenu" runat="server" Title="Offer Letter" OnTabStripCommand="CrewMenu_TabStripCommand"></eluc:TabStrip>

                <table runat="server" id="tblPersonalMaster" width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblFirstName" runat="server" Text="Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblAppliedRank" runat="server" Text="Rank"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtAppliedRank" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblvessel" runat="server" Text="Vessel "></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtvessel" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>

                        <td>
                            <telerik:RadLabel ID="lblWageAgreed" runat="server" Text="Wage Agreed "></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtWageAgreed" Style="text-align: right;" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            <telerik:RadLabel ID="lblcurrency" runat="server"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblContractPeriod" runat="server" Text="Contract Period "></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtContractPeriod" CssClass="readonlytextbox" Style="text-align: right;" ReadOnly="true"></telerik:RadTextBox>
                            <telerik:RadLabel ID="RadLabel1" runat="server" Text="(Days)"></telerik:RadLabel>
                            
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblcomitments" runat="server" Text="Any commitments made : "></telerik:RadLabel>
                            <br />
                            <telerik:RadTextBox runat="server" ID="txtComitmentmade" TextMode="MultiLine" Height="70px"
                                Width="800px">
                            </telerik:RadTextBox>
                            <telerik:RadLabel ID="lblletterid" Visible="false" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                </table>
                <b>
                    <telerik:RadLabel ID="lblHeading" runat="server" Text="Pending Course"></telerik:RadLabel>
                </b>
                <br />
                <div id="divGrid" style="position: relative; z-index: 0">
                    <%--   <asp:GridView Visible="false"  ID="gvOffshoreComponent" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" ShowFooter="true" OnRowCommand="gvOffshoreComponent_RowCommand"
                            OnRowEditing="gvOffshoreComponent_RowEditing" OnRowCancelingEdit="gvOffshoreComponent_RowCancelingEdit"
                            OnRowUpdating="gvOffshoreComponent_RowUpdating" ShowHeader="true" EnableViewState="false"
                            OnRowDataBound="gvOffshoreComponent_RowDataBound" OnRowDeleting="gvOffshoreComponent_OnRowDeleting">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
                    <telerik:RadGrid Visible="false" RenderMode="Lightweight" ID="gvOffshoreComponent" runat="server" Height="500px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvOffshoreComponent_NeedDataSource"
                        OnItemCommand="gvOffshoreComponent_ItemCommand"
                        OnItemDataBound="gvOffshoreComponent_ItemDataBound"
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
                                <%--<asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <FooterStyle Width="100px" HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblCourseName" runat="server">Course</telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCourseNameItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSE") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblPendingCourseEdit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPENDINGCOURSEID") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblPendingCourseEdit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPENDINGCOURSEID") %>'></telerik:RadLabel>
                                        <telerik:RadComboBox Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select" ID="ddlCourseEdit" runat="server" CssClass="input_mandatory"></telerik:RadComboBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadComboBox Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select" ID="ddlCourseAdd" runat="server" CssClass="input_mandatory"></telerik:RadComboBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <FooterStyle Width="100px" HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblArrangedby" runat="server">Arranged By</telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblArrangedbyItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDARRANGEDBY") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadComboBox Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select" Width="100%" ID="ddlArrangedEdit" runat="server">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="-Select-" Value="" />
                                                <telerik:RadComboBoxItem Text="Company" Value="1" />
                                                <telerik:RadComboBoxItem Text="Seafarer" Value="2" />
                                                <telerik:RadComboBoxItem Text="N/A" Value="3" />

                                            </Items>

                                        </telerik:RadComboBox>
                                    </EditItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <FooterStyle Width="100px" HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblDuration" runat="server">Duration</telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDurationItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDURATION") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Number ID="txtDurationEdit" runat="server" Width="95%" MaxLength="7"
                                            IsInteger="true" />
                                    </EditItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <FooterStyle Width="100px" HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblCostPaid" runat="server">Cost is paid by</telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCostPaidItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOSTISPAIDBY") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadComboBox Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select" Width="100%" ID="ddlCostEdit" runat="server">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="-Select-" Value="" />
                                                <telerik:RadComboBoxItem Text="Company" Value="1" />
                                                <telerik:RadComboBoxItem Text="Seafarer" Value="2" />
                                                <telerik:RadComboBoxItem Text="N/A" Value="3" />
                                            </Items>

                                        </telerik:RadComboBox>
                                    </EditItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <FooterStyle Width="100px" HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblAirfareBy" runat="server">Airfare by</telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblAirfareByItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRFAREBY") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadComboBox Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select" Width="100%" ID="ddlAirfareByEdit" runat="server">
                                            <Items>

                                                <telerik:RadComboBoxItem Text="-Select-" Value="" />
                                                <telerik:RadComboBoxItem Text="Company" Value="1" />
                                                <telerik:RadComboBoxItem Text="Seafarer" Value="2" />
                                                <telerik:RadComboBoxItem Text="N/A" Value="3" />
                                            </Items>

                                        </telerik:RadComboBox>
                                    </EditItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <FooterStyle Width="100px" HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblHotelBy" runat="server">Hotel by</telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblHotelByItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHOTELBY") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadComboBox Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select" ID="ddlHotelByEdit" runat="server" Width="100%">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="-Select-" Value="" />
                                                <telerik:RadComboBoxItem Text="Company" Value="1" />
                                                <telerik:RadComboBoxItem Text="Seafarer" Value="2" />
                                                <telerik:RadComboBoxItem Text="N/A" Value="3" />
                                            </Items>

                                        </telerik:RadComboBox>
                                    </EditItemTemplate>

                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <FooterStyle Width="100px" HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblWagesapply" runat="server">Wages apply</telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lbllWagesapplyItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAGESAPPLYYN") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadComboBox Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select" Width="100%" ID="ddlWageApplyEdit" runat="server">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="-Select-" Value="" />
                                                <telerik:RadComboBoxItem Text="Yes" Value="1" />
                                                <telerik:RadComboBoxItem Text="No" Value="2" />
                                                <telerik:RadComboBoxItem Text="N/A" Value="3" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>

                                    <ItemStyle Width="100px" HorizontalAlign="Right"></ItemStyle>
                                    <FooterStyle Width="100px" HorizontalAlign="Right" />
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblECS" runat="server">
                                            Estimated Cost
                                            <br />
                                            for seafarer
                                        </telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblEstimatedcostItem" Style="text-align: right"
                                            runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDESTIMATEDCOST") %>'>
                                        </telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Number ID="txtCostEdit" runat="server" MaxLength="9"
                                            DecimalPlace="2" />
                                    </EditItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblActionHeader" runat="server">Action</telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                            CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>
                                        <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                            ToolTip="Delete"></asp:ImageButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                            CommandName="UPDATE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                            ToolTip="Save"></asp:ImageButton>
                                        <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                            ToolTip="Cancel"></asp:ImageButton>
                                    </EditItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" />

                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="415px" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </div>


                <br />

                <%-- <asp:GridView ID="gvpendingcourse" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowFooter="false" OnRowCommand="gvpendingcourse_RowCommand"
                        OnRowEditing="gvpendingcourse_RowEditing" OnRowCancelingEdit="gvpendingcourse_RowCancelingEdit"
                        OnRowUpdating="gvpendingcourse_RowUpdating" ShowHeader="true" DataKeyNames="FLDDOCUMENTID"
                        OnRowDataBound="gvpendingcourse_RowDataBound" OnRowDeleting="gvpendingcourse_OnRowDeleting"
                        EnableViewState="false" AllowSorting="true">

                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvpendingcourse" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvpendingcourse_NeedDataSource"
                    OnItemCommand="gvpendingcourse_ItemCommand"
                    OnItemDataBound="gvpendingcourse_ItemDataBound"
                    GroupingEnabled="false" EnableHeaderContextMenu="true"
                    AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDDOCUMENTID">
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
                            <telerik:GridTemplateColumn>
                                <HeaderStyle Width="50px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblpendingcouseidedit1" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPENDINGCOURSEID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblselect" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVEYN") %>'></telerik:RadLabel>
                                    <asp:CheckBox ID="chkSelect" runat="server" Enabled="false"/>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblpendingcouseidedit1" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPENDINGCOURSEID") %>'></telerik:RadLabel>
                                    <asp:CheckBox ID="chkSelectedit" runat="server"  />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Course">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Width="150px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCourseNameItem1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblPendingCourse1" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTID") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblPendingCourseEdit1" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblCourseNameItemEdit1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSE") %>'></telerik:RadLabel>
                                </EditItemTemplate>

                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Arranged By">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <HeaderStyle Width="150px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblArrangedbyItem1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDARRANGEDBY") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadComboBox Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select" Width="100%" ID="ddlArrangedEdit1" runat="server">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="-Select-" Value="" />
                                            <telerik:RadComboBoxItem Text="Company" Value="1" />
                                            <telerik:RadComboBoxItem Text="Seafarer" Value="2" />
                                            <telerik:RadComboBoxItem Text="N/A" Value="3" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </EditItemTemplate>

                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Duration">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <HeaderStyle Width="100px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDurationItem1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDURATION") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="txtDurationEdit1" runat="server" Width="95%" MaxLength="7"
                                        IsInteger="true" />
                                </EditItemTemplate>

                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Cost is paid by">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Width="100px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCostPaidItem1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOSTISPAIDBY") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadComboBox Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select" Width="100%" ID="ddlCostEdit1" runat="server">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="-Select-" Value="" />
                                            <telerik:RadComboBoxItem Text="Company" Value="1" />
                                            <telerik:RadComboBoxItem Text="Seafarer" Value="2" />
                                            <telerik:RadComboBoxItem Text="N/A" Value="3" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </EditItemTemplate>

                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Airfare by">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <HeaderStyle Width="100px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblAirfareByItem1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRFAREBY") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadComboBox Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select" Width="100%" ID="ddlAirfareByEdit1" runat="server">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="-Select-" Value="" />
                                            <telerik:RadComboBoxItem Text="Company" Value="1" />
                                            <telerik:RadComboBoxItem Text="Seafarer" Value="2" />
                                            <telerik:RadComboBoxItem Text="N/A" Value="3" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </EditItemTemplate>

                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Hotel by">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Width="100px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblHotelByItem1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHOTELBY") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadComboBox Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select" ID="ddlHotelByEdit1" runat="server" Width="100%">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="-Select-" Value="" />
                                            <telerik:RadComboBoxItem Text="Company" Value="1" />
                                            <telerik:RadComboBoxItem Text="Seafarer" Value="2" />
                                            <telerik:RadComboBoxItem Text="N/A" Value="3" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </EditItemTemplate>

                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Wages Apply">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Width="100px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lbllWagesapplyItem1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAGESAPPLYYN") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadComboBox Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select" Width="100%" ID="ddlWageApplyEdit1" runat="server">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="-Select-" Value="" />
                                            <telerik:RadComboBoxItem Text="Yes" Value="1" />
                                            <telerik:RadComboBoxItem Text="No" Value="2" />
                                            <telerik:RadComboBoxItem Text="N/A" Value="3" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </EditItemTemplate>

                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="  Estimated Cost <br /> for seafarer">

                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <HeaderStyle Width="100px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblEstimatedcostItem1" Style="text-align: right"
                                        runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDESTIMATEDCOST") %>'>
                                    </telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="txtCostEdit1" runat="server" MaxLength="9"
                                        DecimalPlace="2" />
                                </EditItemTemplate>

                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>

                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Edit" 
                                        CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit1"
                                        ToolTip="Edit">
                                         <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                
                                    <asp:LinkButton runat="server" AlternateText="Delete" 
                                        CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete1"
                                        ToolTip="Delete">
                                         <span class="icon"><i class="fa fa-trash"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Save" 
                                        CommandName="UPDATE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave1"
                                        ToolTip="Save">
                                        <span class="icon"><i class="fas fa-save"></i></span>
                                    </asp:LinkButton>
                                  
                                    <asp:LinkButton runat="server" AlternateText="Cancel" 
                                        CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel1"
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
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="2" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>



            </div>
            <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="ucConfirm_Click" OKText="Ok"
                CancelText="Cancel" Visible="false" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
