<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionPNITravelDocumentCheckList.aspx.cs" Inherits="Inspection_InspectionPNITravelDocumentCheckList1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PNI Operation</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="form1" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
         <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvChecklist">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvChecklist"  />
                        <telerik:AjaxUpdatedControl ControlID="lblTotalExpense"  />
                        <telerik:AjaxUpdatedControl ControlID="lblTotalDeduct"  />
                        <telerik:AjaxUpdatedControl ControlID="lblTotalClaim"  />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:UserControlStatus ID="ucStatus" runat="server" />

                <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                <eluc:TabStrip ID="MenuPNIGeneral" TabStrip="true" runat="server" OnTabStripCommand="MenuPNIGeneral_TabStripCommand"></eluc:TabStrip>
                <eluc:TabStrip ID="PNIListMain" runat="server" OnTabStripCommand="PNIListMain_TabStripCommand"></eluc:TabStrip>
                <telerik:RadLabel runat="server" ID="Title1" Text="PNI case Checklist for Operation /  Welfare/Legal & Insurance /Accounts"></telerik:RadLabel>
                <br />
                <telerik:RadLabel runat="server" ID="lblsubtitle" Text="PNI case Checklist for Operation /  Welfare/Legal & Insurance /Accounts"></telerik:RadLabel>


                <table id="tblInspectionPNI" width="100%">
                    <tr>
                        <td>Vessel Name
                        </td>
                        <td>
                            <eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems="true" CssClass="readonlytextbox"
                                VesselsOnly="true" AutoPostBack="true" Width="45%" />
                        </td>
                    </tr>
                    <tr>
                        <td>Name Of The Crew
                        </td>
                        <td>
                            <span id="spnCrewInCharge">
                                <telerik:RadTextBox ID="txtCrewName" runat="server" CssClass="readonlytextbox" Enabled="false"
                                    MaxLength="50" Width="45%">
                                </telerik:RadTextBox>

                                <%-- <img runat="server" alt="" id="imgShowCrewInCharge" style="cursor: pointer; vertical-align: top"
                                        src="<%$ PhoenixTheme:images/picklist.png %>" />--%>
                                <telerik:RadTextBox ID="txtCrewId" runat="server" CssClass="input" MaxLength="20" Width="10px"></telerik:RadTextBox>
                            </span>
                        </td>
                        <td>Name Of On Signer
                        </td>
                        <td>


                            <span id="spnPickListBudgetCode">
                                <telerik:RadTextBox ID="txtonsignername" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="200" Width="45%"></telerik:RadTextBox>
                                <asp:ImageButton runat="server" ID="imgShowcrew" Style="cursor: pointer; vertical-align: top" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                    OnClick="imgShowcrew_Click" ImageAlign="Top" Text=".." />
                                <telerik:RadTextBox ID="txtcrewplanid" AutoPostBack="true" OnTextChanged="textcrewplanid_Changed" runat="server" CssClass="input" MaxLength="200" Width="45%"></telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtemployeeid" runat="server" CssClass="input" MaxLength="20" Width="10px"></telerik:RadTextBox>

                                <telerik:RadTextBox ID="txtrankid" runat="server" CssClass="input" MaxLength="20" Width="10px"></telerik:RadTextBox>


                            </span>
                        </td>


                    </tr>
                    <tr>
                        <td>Rank
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtCrewRank" runat="server" CssClass="readonlytextbox" Enabled="false"
                                MaxLength="50" Width="45%">
                            </telerik:RadTextBox>
                        </td>
                        <td>On Signer's Rank
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtonsignerrank" runat="server" CssClass="readonlytextbox" Enabled="false"
                                MaxLength="50" Width="35%">
                            </telerik:RadTextBox>
                        </td>

                    </tr>
                    <tr>

                        <td>Sign Off Date
                        </td>
                        <td>
                            <eluc:Date ID="txtSignOffDate" runat="server" CssClass="input_mandatory" DatePicker="false" Width="45%" />
                        </td>

                    </tr>
                    <tr>
                        <td>Sign Off Port
                        </td>
                        <td>
                            <eluc:SeaPort ID="ucSignOffPort" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                                Width="45%" />
                        </td>
                        <td>On Signer's Port of Joining
                        </td>
                        <td>
                            <eluc:SeaPort ID="ucSignOffPortonsigner" runat="server" AppendDataBoundItems="true" CssClass="input"
                                Width="35%" />
                        </td>
                    </tr>
                    <tr>
                        <td>Medical Case</td>
                        <td>
                            <telerik:RadComboBox ID="ddlMedicalCase" runat="server" AppendDataBoundItems="true" Width="45%" CssClass="input_mandatory" AutoPostBack="true" EmptyMessage="Type to Select Medical Case" Filter="Contains" MarkFirstMatch="true">
                                <Items>
                                    <telerik:RadComboBoxItem Value="Dummy" Text="--Select--" />
                                    <telerik:RadComboBoxItem Value="0" Text="Yes" />
                                    <telerik:RadComboBoxItem Value="1" Text="No" />
                                </Items>


                            </telerik:RadComboBox>
                        </td>

                    </tr>
                    <tr>
                        <td>Signed-Off Country</td>
                        <td>
                            <telerik:RadTextBox ID="txtCountry" runat="server" CssClass="readonlytextbox" Enabled="false" Width="45%"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Agent Used For
                        </td>
                        <td style="width: 30%">
                            <span id="spnPickListAgent">
                                <telerik:RadTextBox ID="txtAgentNumber" runat="server" Width="60px" CssClass="readonlytextbox" Enabled="False"></telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtAgentName" runat="server" Width="180px" CssClass="readonlytextbox" Enabled="False"></telerik:RadTextBox>

                                <telerik:RadTextBox ID="txtAgent" runat="server" Width="1px" CssClass="input"></telerik:RadTextBox>
                            </span>
                        </td>

                        <td>Agents Contact Details</td>
                        <td>
                            <telerik:RadTextBox ID="txtAgentsContactDetails" runat="server" CssClass="readonlytextbox" Enabled="false" Width="45%"></telerik:RadTextBox></td>
                    </tr>
                    <tr>
                        <td>case report in brief :-
                        </td>
                        <td colspan="3">
                            <telerik:RadTextBox ID="txtDescription" runat="server" CssClass="readonlytextbox" Height="75px" TextMode="MultiLine" Enabled="false"
                                Width="80%">
                            </telerik:RadTextBox>
                        </td>

                    </tr>
                </table>
                <br />
                <asp:Panel ID="pnlChecklist" GroupingText="Voucher Details" runat="server">

                    <eluc:TabStrip ID="Menupni" runat="server" OnTabStripCommand="Menupni_TabStripCommand"></eluc:TabStrip>

                    <%-- <asp:GridView ID="gvChecklist" runat="server" AutoGenerateColumns="False" OnRowCommand="gvChecklist_RowCommand"
                    Font-Size="11px" Width="100%" CellPadding="3" OnRowDataBound="gvChecklist_ItemDataBound"
                    OnRowDeleting="gvChecklist_RowDeleting" ShowHeader="true" EnableViewState="false"
                    OnSelectedIndexChanging="gvChecklist_SelectIndexChanging" AllowSorting="true"
                    ShowFooter="false" OnSorting="gvChecklist_Sorting" DataKeyNames="FLDMEDICALCASEID"
                    OnRowCancelingEdit="gvChecklist_RowCancelingEdit" OnRowUpdating="gvChecklist_RowUpdating"
                    OnRowEditing="gvChecklist_RowEditing">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvChecklist" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                        CellSpacing="0" GridLines="None" OnNeedDataSource="gvChecklist_NeedDataSource"
                        OnItemCommand="gvChecklist_ItemCommand"
                        OnItemDataBound="gvChecklist_ItemDataBound"
                        OnSortCommand="gvChecklist_SortCommand"
                        EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView ShowGroupFooter="true" DataKeyNames="FLDMEDICALCASEID" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed">
                            <GroupByExpressions>
                                <telerik:GridGroupByExpression>
                                    <SelectFields>
                                        <telerik:GridGroupByField FieldName="FLDGROUPBY" FieldAlias="Details" SortOrder="Ascending" />
                                    </SelectFields>
                                    <GroupByFields>
                                        <telerik:GridGroupByField FieldName="FLDGROUPBY" SortOrder="Ascending" />
                                    </GroupByFields>
                                </telerik:GridGroupByExpression>
                            </GroupByExpressions>
                            <HeaderStyle Width="102px" />
                            <Columns>
                                <telerik:GridTemplateColumn Visible="false" UniqueName="FLDGROUPBY" DataField="FLDGROUPBY">
                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPBY") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="S.No" FooterStyle-Width="140px" HeaderStyle-Width="140px"
                                    ItemStyle-Width="140px">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSORTORDERNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <%--<EditItemTemplate>
                                    <telerik:RadLabel ID="lblSNoEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSORTORDER") %>'></telerik:RadLabel>    
                                </EditItemTemplate>--%>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Vouchers">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDepartmentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENT") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblPart" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPART") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblMedicalCaseId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEDICALCASEID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Status">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Status
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblStatusEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSID") %>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                        <telerik:RadComboBox ID="ddlStatus" runat="server" Width="60px" AppendDataBoundItems="true" CssClass="dropdown_mandatory" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                            <Items>
                                                <telerik:RadComboBoxItem Value="Dummy" Text="--Select--" />
                                                <telerik:RadComboBoxItem Value="0" Text="Yes" />
                                                <telerik:RadComboBoxItem Value="1" Text="No" />
                                                <telerik:RadComboBoxItem Value="2" Text="NA" />

                                            </Items>

                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Currency">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Currency
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCY") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblCurrencyID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYID") %>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                        <eluc:UserControlCurrency ID="ucCurrency" runat="server" AppendDataBoundItems="true"
                                            CssClass="dropdown_mandatory" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Actual Amount">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        Actual Amount
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblActualAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTUALAMOUNT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Amount">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        Billable Amount
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblDTKeyEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDID") %>'></telerik:RadLabel>
                                        <eluc:Number ID="ucAmountEdit" runat="server" CssClass="gridinput_mandatory" IsPositive="true"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>' IsInteger="false"
                                            DecimalPlace="2" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Amount in USD" Aggregate="Sum" FooterText="Total:" DataField="FLDUSD">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblAmountInUSD" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSD") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblIDEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDID") %>'></telerik:RadLabel>
                                        <eluc:ToolTip ID="ucToolTipUSD1" runat="server" Text='<%# "Modified By : " + DataBinder.Eval(Container,"DataItem.FLDMODIFIEDBY") + "<BR> Modified Date : " + SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDMODIFIEDDATE")) %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="PO Number">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <eluc:CommonToolTip ID="ucCommonToolTip" runat="server" Screen='<%# "Accounts/AccountsPNIPOInfo.aspx?orderid=" + DataBinder.Eval(Container,"DataItem.FLDID").ToString() +"&PNIID="+DataBinder.Eval(Container,"DataItem.FLDPNIID").ToString() %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action">
                                    <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblActionHeader" runat="server">
                                            Action
                                        </telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>

                                        <asp:LinkButton runat="server" AlternateText="Edit"
                                            CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                            ToolTip="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span></asp:LinkButton>
                                        <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:LinkButton runat="server" AlternateText="Delete"
                                            CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                            ToolTip="Delete">
                                             <span class="icon"><i class="fa fa-trash"></i></span>
                                        </asp:LinkButton>
                                        <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                            CommandName="ATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAttachment"
                                            ToolTip="Attachment"></asp:ImageButton>
                                        <asp:ImageButton runat="server" AlternateText="No Attachment" ImageUrl="<%$ PhoenixTheme:images/no-attachment.png %>"
                                            CommandName="NOATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdNoAttachment"
                                            ToolTip="No Attachment"></asp:ImageButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save"
                                            CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                            ToolTip="Save">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                                        </asp:LinkButton>
                                        <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:LinkButton runat="server" AlternateText="Cancel"
                                            CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                            ToolTip="Cancel">
                                            <span class="icon"><i class="fa fa-trash"></i></span>
                                        </asp:LinkButton>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>

                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvpartD" runat="server" Height="" 
                        CellSpacing="0" GridLines="None" OnNeedDataSource="gvpartD_NeedDataSource"
                       
                       >
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView  HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" 
                            AutoGenerateColumns="false" TableLayout="Fixed">
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="PART D ">
                                    <HeaderStyle Width="75px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblActualAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSNO") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Department - Accounts ">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblActualAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHEAD") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Amount in USD">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblActualAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARTDAMOUNT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                          <%--  <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />--%>
                        </MasterTableView>
                <%--        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true"  ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>--%>
                    </telerik:RadGrid>
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr class="DataGrid-HeaderStyle">
                            <td>PART D
                            </td>
                            <td>Department - Accounts
                            </td>
                            <td>Amount in USD
                            </td>
                        </tr>
                        <tr class="datagrid_alternatingstyle">
                            <td>1
                            </td>
                            <td>Total Expenses Incurred
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblTotalExpense" runat="server"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr class="datagrid_alternatingstyle">
                            <td>2
                            </td>
                            <td>Crew Deductible Amount
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblTotalDeduct" runat="server"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr class="datagrid_alternatingstyle">
                            <td>3
                            </td>
                            <td>Total Claimable Amount
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblTotalClaim" runat="server"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                    <%-- <asp:GridView ID="gvChecklistPartE" runat="server" AutoGenerateColumns="False" OnRowCommand="gvChecklistPartE_RowCommand"
                        Font-Size="11px" Width="100%" CellPadding="3" OnRowDataBound="gvChecklistPartE_ItemDataBound"
                        OnRowDeleting="gvChecklistPartE_RowDeleting" ShowHeader="true" EnableViewState="false"
                        OnSelectedIndexChanging="gvChecklistPartE_SelectIndexChanging" AllowSorting="true"
                        ShowFooter="false" OnSorting="gvChecklist_Sorting" DataKeyNames="FLDMEDICALCASEID"
                        OnRowCancelingEdit="gvChecklistPartE_RowCancelingEdit" OnRowUpdating="gvChecklistPartE_RowUpdating"
                        OnRowEditing="gvChecklistPartE_RowEditing">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvChecklistPartE" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                        CellSpacing="0" GridLines="None" OnNeedDataSource="gvChecklistPartE_NeedDataSource"
                        OnItemCommand="gvChecklistPartE_ItemCommand"
                        OnItemDataBound="gvChecklistPartE_ItemDataBound"
                        OnSortCommand="gvChecklistPartE_SortCommand"
                        EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView ShowGroupFooter="true" DataKeyNames="FLDMEDICALCASEID" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed">
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText= "PART E">
                                     <HeaderStyle Width="75px" />
                                    <itemstyle wrap="False" horizontalalign="Right"></itemstyle>
                                   
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblSNoPartE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSORTORDERNAME") %>'></telerik:RadLabel>
                                </itemtemplate>
                                    <%--<EditItemTemplate>
                                    <telerik:RadLabel ID="lblSNoEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSORTORDER") %>'></telerik:RadLabel>    
                                </EditItemTemplate>--%>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText=" Department - Accounts">
                              
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblDepartmentIdPartE" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENT") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblPartPartE" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPART") %>'></telerik:RadLabel>
                                    <%-- <telerik:RadLabel ID="lblMedicalCaseId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEDICALCASEID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>--%>
                                    <telerik:RadLabel ID="lblNamPartEe" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Date">
                                    <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                    <headertemplate>
                                    Date
                                </headertemplate>
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblDatePartE" runat="server" Enabled="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATE") %>'></telerik:RadLabel>
                                </itemtemplate>
                                    <edititemtemplate>
                                    <eluc:Date ID="ucDate1PartE" runat="server" DatePicker="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATE") %>'
                                        CssClass="input_mandatory" />
                                </edititemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Amount in USD">
                                    <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                    <headertemplate>
                                    Amount in USD
                                </headertemplate>
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblIDE" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblAmountInUSDPartE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                                    <eluc:ToolTip ID="ucToolTipUSD2" runat="server" Text='<%# "Modified By : " + DataBinder.Eval(Container,"DataItem.FLDMODIFIEDBY") + "<BR> Modified Date : " + SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDMODIFIEDDATE")) %>' />
                                    <telerik:RadLabel ID="lblDTKeyE" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                </itemtemplate>
                                    <edititemtemplate>
                                    <eluc:Number ID="ucAmountInUSDPartEEdit" runat="server" CssClass="gridinput_mandatory"
                                        IsPositive="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'
                                        IsInteger="false" DecimalPlace="2" />
                                    <telerik:RadLabel ID="lblDTKeyEEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblIDEEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDID") %>'></telerik:RadLabel>
                                </edititemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action">
                                    <itemstyle wrap="false" horizontalalign="Left" />
                                    <headertemplate>
                                    <telerik:RadLabel ID="lblActionHeader" runat="server">
                                                                    Action
                                    </telerik:RadLabel>
                                </headertemplate>
                                    <itemstyle wrap="False" horizontalalign="Center" width="100px"></itemstyle>
                                    <itemtemplate>
                                    <asp:LinkButton runat="server" AlternateText="Edit"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                        ToolTip="Edit">
                                         <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                    <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:LinkButton runat="server" AlternateText="Delete"
                                        CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                        ToolTip="Delete">
                                         <span class="icon"><i class="fa fa-trash"></i></span>
                                    </asp:LinkButton>
                                    <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                        CommandName="ATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAttachment"
                                        ToolTip="Attachment"></asp:ImageButton>
                                    <asp:ImageButton runat="server" AlternateText="No Attachment" ImageUrl="<%$ PhoenixTheme:images/no-attachment.png %>"
                                        CommandName="NOATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdNoAttachment"
                                        ToolTip="No Attachment"></asp:ImageButton>
                                </itemtemplate>
                                    <edititemtemplate>
                                    <asp:LinkButton runat="server" AlternateText="Save"
                                        CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                        ToolTip="Save">
                                        <span class="icon"><i class="fas fa-save"></i></span>
                                    </asp:LinkButton>
                                    <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:LinkButton runat="server" AlternateText="Cancel"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel">                                        
                                        <span class="icon"><i class="fa fa-trash"></i></span>
                                    </asp:LinkButton>
                                </edititemtemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true"  ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </asp:Panel>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>


</html>
