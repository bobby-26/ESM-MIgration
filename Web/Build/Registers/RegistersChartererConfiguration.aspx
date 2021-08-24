<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersChartererConfiguration.aspx.cs"
    Inherits="RegistersChartererConfiguration" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TrainingMatrix" Src="~/UserControls/UserControlTrainingMatrixStandard.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Charterer Configuration</title> 
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
      <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmChartererConfiguration" runat="server">
<telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
  <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
                  
                    <eluc:TabStrip ID="CharterConfigMenu" runat="server" OnTabStripCommand="CharterConfigMenu_TabStripCommand">
                    </eluc:TabStrip>
        
        		<table id="tblTable" runat="server" width="100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCharterer" runat="server" Text="Charterer"	/>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtCharterer" Width="300px" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblShortName" runat="server" Text="Short Name"	/>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtShortName" MaxLength="6" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblChartererStandard" runat="server" Text="Training Matrix Identifier"	/>
                            </td>
                            <td>
                                <eluc:TrainingMatrix ID="ucChartererStanard" AutoPostBack="true" Width="200px" AppendDataBoundItems="true"
                                    runat="server" CssClass="input" AddressType='<%# ((int)PhoenixAddressType.CHARTERER).ToString() %>' />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblownstandard" runat="server" Text="Has own T&Q matrix standard?"	/>
                            </td>
                            <td>
                                <asp:CheckBox ID="ckbownstandard" runat="server" />
                            </td>
                        </tr>
                    </table>

                    <eluc:TabStrip ID="MenuChartererConfiguration" runat="server" Visible="false" OnTabStripCommand="MenuChartererConfiguration_TabStripCommand">
                    </eluc:TabStrip>


                  <%--  <asp:GridView ID="gvChartererConfiguration" runat="server" AutoGenerateColumns="False"
                        Font-Size="11px" Width="100%" CellPadding="3" ShowFooter="true" OnRowCommand="gvChartererConfiguration_RowCommand"
                        OnRowEditing="gvChartererConfiguration_RowEditing" OnRowCancelingEdit="gvChartererConfiguration_RowCancelingEdit"
                        OnRowUpdating="gvChartererConfiguration_RowUpdating" ShowHeader="true" EnableViewState="false"
                        DataKeyNames="FLDCONFIGURATIONID" OnRowDataBound="gvChartererConfiguration_RowDataBound"
                        OnRowDeleting="gvChartererConfiguration_OnRowDeleting">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <telerik:GridTemplateColumn	HeaderText="Rank Group">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRankGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadComboBox Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select sub category" ID="ddlRankGroupEdit" runat="server" CssClass="input_mandatory">
                                    </telerik:RadComboBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                   <telerik:RadComboBox Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select sub category" ID="ddlRankGroupAdd" runat="server" CssClass="input_mandatory">
                                   </telerik:RadComboBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblContractPeriodHeader" runat="server" Text="Contract Period (in days)"></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblContractPeriod" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTRACTPERIOD") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="ucContractPeriodEdit" runat="server" IsInteger="true" MaxLength="3"
                                        CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTRACTPERIOD") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number ID="ucContractPeriodAdd" runat="server" IsInteger="true" MaxLength="3"
                                        CssClass="input_mandatory" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblRangeHeader" runat="server" Text="Range (+/- days)"></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRange" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANGE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="ucRangeEdit" runat="server" IsInteger="true" MaxLength="3" CssClass="input_mandatory"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANGE") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number ID="ucRangeAdd" runat="server" IsInteger="true" MaxLength="3" CssClass="input_mandatory" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblActionHeader" runat="server">Action</telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="UPDATE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="Add" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>	--%>

                      <telerik:RadGrid	ID="gvChartererConfiguration" runat="server" AutoGenerateColumns="False"
                        Font-Size="11px" Width="100%" CellPadding="3" ShowFooter="true" OnItemCommand="gvChartererConfiguration_ItemCommand"
                        ShowHeader="true" EnableViewState="false"      OnItemDataBound="gvChartererConfiguration_ItemDataBound" 
                     OnSorting="gvDocumentTestQuestions_Sorting"          AllowSorting="true" 
                        AllowPaging="true" AllowCustomPaging="true" GridLines="None" 
                        OnNeedDataSource="gvChartererConfiguration_NeedDataSource" RenderMode="Lightweight"
                        GroupingEnabled="false" EnableHeaderContextMenu="true">
                       
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                    HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed"      DataKeyNames="FLDCONFIGURATIONID"  >
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="RadLabel1" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />


			        <Columns>
                           
                            <telerik:GridTemplateColumn HeaderText="Max Tour of Duty  (in days)"	AllowSorting="true"	SortExpression="FLDMAXTOURDUTY">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Wrap="true" HorizontalAlign="Left" Width="40%" />
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />

				            <ItemTemplate>
                                    <telerik:RadLabel ID="lblRankGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAXTOURDUTY") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                   <eluc:Number ID="ucmaxtourdutyEdit" runat="server" IsInteger="true" MaxLength="3"     Width="100%" 
                                        CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAXTOURDUTY") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number ID="ucmaxtourdutyAdd" runat="server" IsInteger="true" MaxLength="3"     Width="100%" 
                                        CssClass="input_mandatory"  />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Certificate Validity (in days)"	AllowSorting="true"	SortExpression="FLDCERTVALIDITY">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Wrap="true" HorizontalAlign="Left" Width="40%" />
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />

				                <ItemTemplate>
                                    <telerik:RadLabel ID="lblContractPeriod" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTVALIDITY") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="ucCertPeriodEdit" runat="server" IsInteger="true" MaxLength="3"
                                        CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTVALIDITY") %>' Width="100%" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number ID="ucCertPeriodAdd" runat="server" IsInteger="true" MaxLength="3"  Width="100%" 
                                        CssClass="input_mandatory" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                           <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <FooterStyle Wrap="true" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center" ></ItemStyle>
                                <ItemTemplate>
                                     <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit"
                                    ToolTip="Edit">
                                         <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete">
                                         <span class="icon"><i class="fa fa-trash"></i></span>
                                </asp:LinkButton>
                                
                                </ItemTemplate>
                                 <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave"
                                    ToolTip="Save">
                                        <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel"
                                    ToolTip="Cancel">
                                        <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ToolTip="Add New" CommandName="Add"
                                    ID="cmdAdd">
                                        <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                    AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4"
                        ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
     
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>