<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreReimbursement.aspx.cs" Inherits="CrewOffshoreReimbursement" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.VesselAccounts" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reimbursment</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <style type="text/css">
            .hidden {
                display: none;
            }
        </style>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmCrewList" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

                <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />



                <eluc:TabStrip ID="MenuReimbursementGeneral" runat="server" TabStrip="true" OnTabStripCommand="MenuReimbursementGeneral_TabStripCommand"></eluc:TabStrip>

                <br />
                <div>
                    <table width="60%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Vessel ID="UcVessel" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                    AutoPostBack="true" OnTextChangedEvent="ucVessel_Changed" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblMonth" runat="server" Text="Month"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlMonth" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged" Filter="Contains" EmptyMessage="Type to select month" MarkFirstMatch="true">
                                    <Items>

                                        <telerik:RadComboBoxItem Text="January" Value="1" />
                                        <telerik:RadComboBoxItem Text="February" Value="2" />
                                        <telerik:RadComboBoxItem Text="March" Value="3" />
                                        <telerik:RadComboBoxItem Text="April" Value="4" />
                                        <telerik:RadComboBoxItem Text="May" Value="5" />
                                        <telerik:RadComboBoxItem Text="June" Value="6" />
                                        <telerik:RadComboBoxItem Text="July" Value="7" />
                                        <telerik:RadComboBoxItem Text="August" Value="8" />
                                        <telerik:RadComboBoxItem Text="September" Value="9" />
                                        <telerik:RadComboBoxItem Text="October" Value="10" />
                                        <telerik:RadComboBoxItem Text="November" Value="11" />
                                        <telerik:RadComboBoxItem Text="December" Value="12" />
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblYear" runat="server" Text="Year"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlYear" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" Filter="Contains" EmptyMessage="Type to select month" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                    </table>
                </div>

                <eluc:TabStrip ID="MenuReimbursement" runat="server" OnTabStripCommand="MenuReimbursement_TabStripCommand"></eluc:TabStrip>

                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <%--<asp:GridView ID="gvRem" runat="server" AutoGenerateColumns="False" Width="100%" OnSelectedIndexChanging="gvRem_SelectedIndexChanging"
                        OnRowCommand="gvRem_RowCommand" OnRowEditing="gvRem_RowEditing" OnRowCancelingEdit="gvRem_RowCancelingEdit" OnRowDeleting="gvRem_RowDeleting"
                        OnRowUpdating="gvRem_RowUpdating" OnRowDataBound="gvRem_RowDataBound" AllowSorting="true" OnSorting="gvRem_Sorting"
                        CellPadding="3" ShowFooter="true" ShowHeader="true" EnableViewState="true" DataKeyNames="FLDREIMBURSEMENTID">
                        <FooterStyle CssClass="datagrid_footerstyle" />
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvRem" runat="server" Height="500px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvRem_NeedDataSource"
                        OnItemCommand="gvRem_ItemCommand"
                        OnItemDataBound="gvRem_ItemDataBound"
                        OnSortCommand="gvRem_SortCommand"

                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                  
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDREIMBURSEMENTID" CommandItemDisplay="Top">
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
                            <CommandItemSettings ShowRefreshButton="false" ShowAddNewRecordButton="false" />
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Ref. No">
                                 <HeaderStyle Width="100px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkRefNo" runat="server" CommandName="SELECT" Text='<%# ((DataRowView)Container.DataItem)["FLDREFERENCENO"]%>'></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="File No">
                                  <HeaderStyle Width="100px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDFILENO"]%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Employee Name">
                                   <HeaderStyle Width="150px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblEmpName" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDEMPLOYEENAME"]%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblEmpIDEdit" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDEMPLOYEEID"]%>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblSignOnOffId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNONOFFID"]%>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblEmpNameEdit" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDEMPLOYEENAME"]%>'></telerik:RadLabel>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadComboBox Filter="Contains" EmptyMessage="Type to select month" MarkFirstMatch="true" ID="ddlEmployeeAdd" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"></telerik:RadComboBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Rank">
                                    <HeaderStyle Width="75px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDRANKCODE"].ToString()%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Account of">
                                     <HeaderStyle Width="100px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDACCOUNTOF"].ToString()%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Reimbursement/Recovery">
                                     <HeaderStyle Width="100px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                  
                                    <ItemTemplate>
                                        <%# GetName(((DataRowView)Container.DataItem)["FLDEARNINGDEDUCTION"].ToString())%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadComboBox ID="ddlEarDed" runat="server" CssClass="input_mandatory" OnSelectedIndexChanged="ddlEarDed_SelectedIndexChanged" AutoPostBack="true"
                                            Filter="Contains" EmptyMessage="Type to select month" MarkFirstMatch="true" Width="100%">
                                            <Items>

                                                <telerik:RadComboBoxItem Text="--Select--" Value="" />
                                                <telerik:RadComboBoxItem Text="Reimbursement" Value="2" />
                                                <telerik:RadComboBoxItem Text="Recovery" Value="-2" />
                                            </Items>

                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadComboBox ID="ddlEarDedAdd" runat="server" CssClass="input_mandatory" OnSelectedIndexChanged="ddlEarDed_SelectedIndexChanged" AutoPostBack="true"
                                            Filter="Contains" EmptyMessage="Type to select month" MarkFirstMatch="true" Width="100%">
                                            <Items>

                                                <telerik:RadComboBoxItem Text="--Select--" Value="" />
                                                <telerik:RadComboBoxItem Text="Reimbursement" Value="2" />
                                                <telerik:RadComboBoxItem Text="Recovery" Value="-2" />
                                            </Items>

                                        </telerik:RadComboBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Purpose">
                                    <HeaderStyle Width="100px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDHARDNAME"]%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Hard ID="ddlDesc" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" HardTypeCode="128" ShortNameFilter="TRV,USV,AFR,EBG,CFE,LFE,MEF"
                                            HardList='<%#PhoenixRegistersHard.ListHard(1, 128, 0, "TRV,USV,AFR,EBG,CFE,LFE,MEF")%>' Width="100%"/>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Hard ID="ddlDescAdd" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" HardTypeCode="128" ShortNameFilter="TRV,USV,AFR,EBG,CFE,LFE,MEF"
                                            HardList='<%#PhoenixRegistersHard.ListHard(1, 128, 0, "TRV,USV,AFR,EBG,CFE,LFE,MEF")%>' Width="100%"/>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Description">
                                   <HeaderStyle Width="150px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDDESCRIPTION"]%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtDescriptionEdit" runat="server" CssClass="gridinput_mandatory" MaxLength="500" Text='<%# ((DataRowView)Container.DataItem)["FLDDESCRIPTION"]%>' Width="100%"></telerik:RadTextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadTextBox ID="txtDescriptionAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="500" Width="100%"></telerik:RadTextBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Currency">
                                      <HeaderStyle Width="100px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDCURRENCYCODE"]%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Currency ID="ddlCurrency" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                            CurrencyList="<%#PhoenixRegistersCurrency.ListCurrency(1) %>" Width="100%" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Currency ID="ddlCurrencyAdd" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                            CurrencyList="<%#PhoenixRegistersCurrency.ListCurrency(1) %>" Width="100%" />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Amount">
                                      <HeaderStyle Width="75px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDAMOUNT"]%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Number ID="txtAmount" runat="server" CssClass="input_mandatory" Width="100%"
                                            Text='<%# ((DataRowView)Container.DataItem)["FLDAMOUNT"]%>' />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Number ID="txtAmountAdd" runat="server" CssClass="input_mandatory" Width="100%" />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Amount (USD)">
                                      <HeaderStyle Width="75px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDAMOUNTUSD"]%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Claim Submission Date">
                                     <HeaderStyle Width="150px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblContractDate" runat="server" Visible="true" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDSUBMISSIONDATE")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Date ID="ucDateEdit" Width="100%" runat="server" CssClass="input_mandatory" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDSUBMISSIONDATE")) %>' />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Date ID="ucDateAdd" Width="100%" runat="server" CssClass="input_mandatory" />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Budget Code">
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <span id="spnPickListBudgetEdit">
                                            <telerik:RadTextBox ID="txtBudgetCodeEdit" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'
                                                MaxLength="20" Width="80%"></telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtBudgetNameEdit" runat="server" Width="0px" CssClass="input hidden" Enabled="False"></telerik:RadTextBox>
                                            <asp:LinkButton ID="btnShowBudgetEdit" runat="server"
                                                ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" >
                                                <span class="icon"><i class="fas fa-tasks-picklist"></i></span>
                                            </asp:LinkButton>
                                            <telerik:RadTextBox ID="txtBudgetIdEdit" runat="server" Width="0px" CssClass="input hidden" Text='<%#DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE") %>'></telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtBudgetgroupIdEdit" runat="server" Width="0px" CssClass="input hidden"></telerik:RadTextBox>
                                        </span>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <span id="spnPickListBudgetAdd">
                                            <telerik:RadTextBox ID="txtBudgetCodeAdd" runat="server"
                                                MaxLength="20" Width="80%"></telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtBudgetNameAdd" runat="server" Width="0px" CssClass="input hidden" Enabled="False"></telerik:RadTextBox>
                                            <asp:LinkButton ID="btnShowBudgetAdd" runat="server" 
                                                ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" >
                                                  <span class="icon"><i class="fas fa-tasks-picklist"></i></span>
                                            </asp:LinkButton>
                                            <telerik:RadTextBox ID="txtBudgetIdAdd" runat="server" Width="0px" CssClass="input hidden"></telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtBudgetgroupIdAdd" runat="server" Width="0px" CssClass="input hidden"></telerik:RadTextBox>
                                        </span>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <HeaderStyle Width="100px" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblActionHeader" runat="server">Action</telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Edit" 
                                            CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                            ToolTip="Edit">
                                             <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>
                                       
                                        <asp:LinkButton runat="server" AlternateText="Delete" 
                                            CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                            ToolTip="Delete">
                                             <span class="icon"><i class="fa fa-trash"></i></span>
                                        </asp:LinkButton>
                                      
                                        <asp:LinkButton runat="server" AlternateText="Attachment" 
                                            CommandName="Attachment" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAtt"
                                            ToolTip="Attachment">
                                            <span class="icon"><i class="fas fa-paperclip"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save" 
                                            CommandName="Update" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                            ToolTip="Save">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                                        </asp:LinkButton>
                                        <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                        <asp:LinkButton runat="server" AlternateText="Cancel" 
                                            CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                            ToolTip="Cancel">
                                            <span class="icon"><i class="fas fa-times-circle"></i></span>
                                        </asp:LinkButton>
                                    </EditItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                    <FooterTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save" 
                                            CommandName="Add" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAdd"
                                            ToolTip="Add New">
                                              <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                        </asp:LinkButton>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="415px" EnableNextPrevFrozenColumns="true" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>

                </div>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
