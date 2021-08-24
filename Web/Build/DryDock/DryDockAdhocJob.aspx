<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DryDockAdhocJob.aspx.cs" Inherits="DryDockAdhocJob" EnableEventValidation="true" ValidateRequest="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Unit" Src="~/UserControls/UserControlUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Responsibilty" Src="~/UserControls/UserControlDiscipline.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Adhoc Job</title>
    <telerik:RadCodeBlock runat="server">
         <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
         <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvComponent.ClientID %>"));
                }, 200);
                 setTimeout(function () {
                   TelerikGridResize($find("<%= gvAdhocJobLineItem.ClientID %>"));
                }, 200);
           }
           window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
               Resize();
           }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRepairJob" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxPanel runat="server" ID="radajaxpanel">
      
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            
                <eluc:TabStrip ID="MenuHeader" runat="server" OnTabStripCommand="MenuHeader_TabStripCommand"
                    TabStrip="true"></eluc:TabStrip>
           
           

                <eluc:TabStrip ID="AdhocJobSpecification" runat="server" OnTabStripCommand="AdhocJobSpecification_TabStripCommand"></eluc:TabStrip>

            <table width="100%" cellpadding="2" cellspacing="3">
                <tr>
                    <td>
                        <telerik:RadTextBox ID="txtprojectid" runat="server" Visible="false"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtreferencejobid" runat="server" Visible="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:radlabel    ID="lblRegister" runat="server" Text="Register"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlRegister" runat="server" OnSelectedIndexChanged="ddlRegister_SelectedIndexChanged" CssClass="input" AutoPostBack="true">
                            <Items>
                                <telerik:DropDownListItem Text="--Select--" Value="DUMMY" />
                                <telerik:DropDownListItem Text="General Service" Value="1" />
                                <telerik:DropDownListItem Text="Standard Job" Value="2" />
                                <telerik:DropDownListItem Text="Repair Job" Value="0" />
                            </Items>

                        </telerik:RadDropDownList>
                        <telerik:radlabel    ID="lblJobRegister" runat="server" Visible="false"></telerik:RadLabel>
                    </td>
                    <td rowspan="13">
                        <b>
                            <telerik:radlabel    ID="lblInclude" runat="server" Text="Include"></telerik:RadLabel>
                        </b>
                        <telerik:RadListBox CheckBoxes="true" ID="cblInclude" runat="server" RepeatDirection="Vertical">
                        </telerik:RadListBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:radlabel    ID="lblJob" runat="server" Text="Job Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlJob" runat="server" OnSelectedIndexChanged="ddlJob_SelectedIndexChanged" CssClass="dropdown_mandatory" AutoPostBack="true" Visible="false">
                        </telerik:RadComboBox>
                        <telerik:RadTextBox ID="txtJobnumber" runat="server" Visible="false" CssClass="input_mandatory"></telerik:RadTextBox>
                        <telerik:radlabel    ID="lblJobnumber" runat="server" Visible="false"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:radlabel    ID="lblJobType" runat="server" Text="Job Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlJobType" DefaultMessage="--Select--" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true">
                        </telerik:RadComboBox>
                        <telerik:radlabel    ID="lblAdhocJobType" runat="server" Visible="false"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:radlabel    ID="lblJobStatus" runat="server" Text="Status"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:radLabel    runat="server" ID="lblStatus" MaxLength="10"></telerik:radLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:radLabel    ID="lblTitle" runat="server" Text="Title"></telerik:radLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtTitle" CssClass="input_mandatory" MaxLength="100"
                            Width="360px">
                        </telerik:RadTextBox>
                        <telerik:radLabel    ID="lblJobTitle" runat="server" Visible="false"></telerik:radLabel>
                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <telerik:radLabel    ID="lblJobDescription" runat="server" Text="Job Description"></telerik:radLabel>

                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtJobDescription" runat="server" CssClass="input_mandatory" Width="60%"
                            TextMode="MultiLine" Rows="6">
                        </telerik:RadTextBox>
                        <div id="divdes" runat="server" style="width: 600px; height: 100px; overflow-y: scroll; border: 1px solid grey" visible="false">
                            <telerik:radLabel    ID="lbldescription" runat="server" Visible="false"></telerik:radLabel>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:radLabel    ID="lblworkrequests" runat="server" Text="Work Requests"></telerik:radLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkworkrequests" runat="server" AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:radLabel    ID="lblLocation" runat="server" Text="Location"></telerik:radLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLocation" runat="server" CssClass="input" TextMode="MultiLine" Rows="2" Width="360px"></telerik:RadTextBox>
                        <div id="divloc" runat="server" style="width: 400px; height: 50px; overflow-y: scroll; border: 1px solid grey" visible="false">
                            <telerik:radLabel    ID="lblJobLocation" runat="server" Visible="false"></telerik:radLabel>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:radLabel    ID="lblResponsibilty" runat="server" Text="Responsibilty"></telerik:radLabel>

                    </td>
                    <td>
                        <telerik:RadListBox CheckBoxes="true" ID="cblResponsibilty" runat="server" RepeatDirection="Horizontal">
                        </telerik:RadListBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:radLabel    ID="lblPlannedStartDate" runat="server" Text="Planned Start Date"></telerik:radLabel>

                    </td>
                    <td>
                        <eluc:Date ID="ucStartDate" runat="server" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:radLabel    ID="lblCostClassification" runat="server" Text="Cost Classification"></telerik:radLabel>

                    </td>
                    <td>
                        <telerik:RadDropDownList DefaultMessage="--Select--" ID="ddlCostClassification" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true">
                        </telerik:RadDropDownList>
                        <telerik:radLabel    ID="lblJobCostClassification" runat="server" Visible="false"></telerik:radLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:radLabel    ID="lblWorktobesurveyedby" runat="server" Text="Work to be surveyed by"></telerik:radLabel>

                    </td>
                    <td>
                        <telerik:RadListBox CheckBoxes="true" ID="cblWorkSurvey" runat="server" RepeatDirection="Horizontal">
                        </telerik:RadListBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:radLabel    ID="lblMaterial" runat="server" Text="Material"></telerik:radLabel>

                    </td>
                    <td colspan="2">
                        <telerik:RadListBox CheckBoxes="true" ID="cblMaterial" runat="server" RepeatDirection="Horizontal">
                        </telerik:RadListBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:radLabel    ID="lblEnclosed" runat="server" Text="Enclosed"></telerik:radLabel>

                    </td>
                    <td colspan="2">
                        <telerik:RadListBox CheckBoxes="true" ID="cblEnclosed" runat="server" RepeatDirection="Horizontal">
                        </telerik:RadListBox>
                    </td>
                </tr>
            </table>
            <hr />
           
            <telerik:RadGrid RenderMode="Lightweight" ID="gvComponent" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None"
                OnNeedDataSource="gvComponent_NeedDataSource"
                OnItemDataBound="gvComponent_ItemDataBound"
                OnItemCommand="gvComponent_ItemCommand"
                OnUpdateCommand ="gvComponent_UpdateCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                <MasterTableView ShowFooter="true" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDDTKEY" TableLayout="Fixed" CommandItemDisplay="None" >
                     <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:radLabel  ForeColor="Black"   ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:radLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Component" HeaderStyle-HorizontalAlign="Center"> 

                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:radlabel    ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:radlabel    ID="lblComponentid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></telerik:RadLabel>
                                <%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNUMBER")%> - <%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListComponent">
                                    <telerik:RadTextBox ID="txtComponentCode" runat="server" CssClass="input_mandatory" MaxLength="20"
                                         Width="60px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>'>
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtComponentName" runat="server" CssClass="input_mandatory" MaxLength="20"
                                         Width="210px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'>
                                    </telerik:RadTextBox>
                                      <asp:ImageButton ID="imgComponent" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="Top" Text=".." />
                                    <telerik:RadTextBox ID="txtComponentId" runat="server" CssClass="hidden" Width="0px" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTID")%>'></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <span id="spnPickListComponentAdd">
                                    <telerik:RadTextBox ID="txtComponentCodeAdd" runat="server" CssClass="input_mandatory" MaxLength="20"
                                         Width="60px">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtComponentNameAdd" runat="server" CssClass="input_mandatory" MaxLength="20"
                                         Width="210px">
                                    </telerik:RadTextBox>
                                       <asp:ImageButton ID="imgComponentAdd" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="Top" Text=".." />
                                    <telerik:RadTextBox ID="txtComponentIdAdd" runat="server" CssClass="hidden" Width="0px"></telerik:RadTextBox>
                                </span>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Specification"   HeaderStyle-HorizontalAlign="Center"> 
                            <HeaderTemplate>
                                <telerik:radLabel    ID="lblHeaderSpecification" runat="server" Text="Specification"></telerik:radLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:radLabel    ID="lblDescription" runat="server"></telerik:radLabel>
                                <eluc:ToolTip runat="server" ID="ucTooltipDesc" Text='' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtDescription" runat="server" CssClass="gridinput" Text='<%# General.SanitizeHtml(DataBinder.Eval(Container, "DataItem.FLDSPECIFICATION").ToString())%>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action"  HeaderStyle-HorizontalAlign="Center"> 
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:radlabel    ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Edit" 
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCompEdit"
                                    ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Delete" 
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCompDelete"
                                    ToolTip="Delete">
                                     <span class="icon"><i class="fa fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save" 
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCompSave"
                                    ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Cancel" 
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCompCancel"
                                    ToolTip="Cancel">
                                     <span class="icon"><i class="fa fa-trash"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save" 
                                    CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCompAdd"
                                    ToolTip="Add New">
                                     <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <hr />
            <div style="position: relative;">
                <eluc:TabStrip ID="MenuDryDockAdhocLineItem" runat="server" OnTabStripCommand="DryDockAdhocJobLineItem_TabStripCommand"></eluc:TabStrip>
            </div>
          
                <%--<asp:GridView ID="gvAdhocJobLineItem" runat="server" AutoGenerateColumns="False"
                    Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvAdhocJobLineItem_RowCommand"
                    OnRowDataBound="gvAdhocJobLineItem_ItemDataBound" OnRowCancelingEdit="gvAdhocJobLineItem_RowCancelingEdit"
                    OnRowDeleting="gvAdhocJobLineItem_RowDeleting" OnRowEditing="gvAdhocJobLineItem_RowEditing"
                    ShowFooter="true" ShowHeader="true" EnableViewState="false" OnSorting="gvAdhocJobLineItem_Sorting"
                    AllowSorting="true" OnRowCreated="gvAdhocJobLineItem_RowCreated" OnSelectedIndexChanging="gvAdhocJobLineItem_SelectedIndexChanging"
                    OnRowUpdating="gvAdhocJobLineItem_RowUpdating">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="10px" />
                    <Columns>--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvAdhocJobLineItem" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None"
                    
                    OnNeedDataSource="gvAdhocJobLineItem_NeedDataSource"
                    OnItemDataBound="gvAdhocJobLineItem_ItemDataBound1" GroupingEnabled="false"
                    OnItemCommand ="gvAdhocJobLineItem_ItemCommand" EnableHeaderContextMenu="true"
                    OnUpdateCommand="gvAdhocJobLineItem_UpdateCommand">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                    <MasterTableView ShowFooter="true" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDJOBDETAILID" TableLayout="Fixed" CommandItemDisplay="Top">
                        <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:radLabel ForeColor="Black"  ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:radLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>

                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Job Detail">
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <headertemplate>
                                <telerik:radlabel    ID="lblJobDetail" runat="server" Text="Job Detail"></telerik:RadLabel>

                            </headertemplate>
                                <itemtemplate>
                                <telerik:radlabel    ID="lbljobdetailid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBDETAILID") %>'></telerik:RadLabel>
                                <telerik:radlabel    ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:radlabel    ID="lblDetail" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                            </itemtemplate>
                                <edititemtemplate>
                                <telerik:radlabel    ID="lbljobdetailidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBDETAILID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtDetailEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'
                                    CssClass="gridinput_mandatory" ToolTip="Job Detail"></telerik:RadTextBox>
                            </edititemtemplate>
                                <footertemplate>
                                <telerik:RadTextBox ID="txtDetailAdd" runat="server" CssClass="gridinput_mandatory"
                                    Width="90%" ToolTip="Job Detail"></telerik:RadTextBox>
                            </footertemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Unit">
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <headertemplate>
                                <telerik:radlabel    ID="lblUnit" runat="server" Text="Unit"></telerik:RadLabel>

                            </headertemplate>
                                <itemtemplate>
                                <telerik:radlabel    ID="lblUnit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></telerik:RadLabel>
                            </itemtemplate>
                                <edititemtemplate>
                                <eluc:Unit ID="ucUnitEdit" CssClass="input" runat="server" AppendDataBoundItems="true"
                                    UnitList='<%# PhoenixRegistersUnit.ListUnit()%>' SelectedUnit='<%# DataBinder.Eval(Container,"DataItem.FLDUNIT") %>' />
                            </edititemtemplate>
                                <footertemplate>
                                <eluc:Unit ID="ucUnitAdd" CssClass="input" runat="server" AppendDataBoundItems="true" Width="100%"
                                    UnitList='<%# PhoenixRegistersUnit.ListUnit()%>' />
                            </footertemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <headerstyle horizontalalign="Center" verticalalign="Middle"></headerstyle>
                                <headertemplate>
                                <telerik:Radlabel    ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:Radlabel>
                            </headertemplate>
                                <itemstyle wrap="False" horizontalalign="Center" width="100px"></itemstyle>
                                <itemtemplate>
                                <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Edit" 
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Delete" 
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="Delete">
                                    
 <span class="icon"><i class="fa fa-trash"></i></span>
                                </asp:LinkButton>
                            </itemtemplate>
                                <edititemtemplate>
                                <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save"
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                    ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Cancel" 
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel">
                                     <span class="icon"><i class="fa fa-trash"></i></span>
                                </asp:LinkButton>
                            </edititemtemplate>
                                <footerstyle horizontalalign="Center" />
                                <footertemplate>
                                <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save" 
                                    CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                    ToolTip="Add New">
                                     <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </footertemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                          <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            
            <asp:Button runat="server" ID="cmdHiddenPick" OnClick="cmdHiddenPick_Click" />
            <eluc:Status ID="ucStatus" runat="server" />
            <eluc:ConfirmMessage ID="ucConfirmMessage" runat="server" Text="" Visible="false" OnConfirmMesage="ucConfirmMessage_ConfirmMessage" style="z-index: 99" />
       
            </telerik:RadAjaxPanel>
    </form>
</body>
</html>
