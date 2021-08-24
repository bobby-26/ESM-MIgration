<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DryDockJobGeneral.aspx.cs" Inherits="DryDockJobGeneral" ValidateRequest="true" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Unit" Src="~/UserControls/UserControlUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Responsibilty" Src="~/UserControls/UserControlDiscipline.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="rad1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    
    <script type="text/javascript" language="javascript">
        function selectJobDetail(jobdetailid, obj) {

           
            var chk = $find(obj.id);
           
            AjxPost("functionname=selectjobdetail|jobdetailid=" + jobdetailid + "|checked=" + !chk.get_checked(), SitePath + "PhoenixWebFunctions.aspx", null, false);
        }
    </script>
        </telerik:RadCodeBlock>
    <style type="text/css">
        .hidden {
            visibility: hidden;
        }
    </style>
</head>    
<body>
    <form id="frmStandardJobGeneral" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; Width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>


            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuHeader" runat="server" OnTabStripCommand="MenuHeader_TabStripCommand"
                    TabStrip="true"></eluc:TabStrip>
            </div>
            <div style="font-weight: 600; font-size: 12px;" runat="server">

                <eluc:TabStrip ID="MenuStandardJobSpecification" runat="server" OnTabStripCommand="StandardJobSpecification_TabStripCommand"></eluc:TabStrip>

            </div>
             <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" >
            <table Width="100%" cellpadding="1" cellspacing="3">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblJobType" runat="server" Text="Job Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlJobType" runat="server" CssClass="dropdown_mandatory" DefaultMessage="--Select--">
                        </telerik:RadDropDownList>
                    </td>
                    <td rowspan="13">
                        <b>
                            <telerik:RadLabel ID="lblInclude" runat="server" Text="Include"></telerik:RadLabel>
                        </b>
                        <telerik:RadListBox CheckBoxes="true" ID="cblInclude" runat="server" RepeatDirection="Vertical">
                        </telerik:RadListBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtNumber" CssClass="input_mandatory" MaxLength="10"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtTitle" CssClass="input_mandatory" MaxLength="100"
                            Width="360px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <%--  <tr>
                        <td>
                            Component
                        </td>
                        <td>
                            <span id="spnPickListComponent">
                                <asp:TextBox ID="txtComponentCode" runat="server" CssClass="input" MaxLength="20"
                                    Enabled="false" Width="60px"></asp:TextBox>
                                <asp:TextBox ID="txtComponentName" runat="server" CssClass="input" MaxLength="20"
                                    Enabled="false" Width="210px"></asp:TextBox>
                                <img id="imgComponent" runat="server" onclick="return showPickList('spnPickListComponent', 'codehelp1', '', '../Common/CommonPickListComponent.aspx', true); "
                                    src="<%$ PhoenixTheme:images/picklist.png %>" style="cursor: pointer; vertical-align: top" />
                                <asp:TextBox ID="txtComponentId" runat="server" CssClass="input" Width="10px"></asp:TextBox>
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Specification
                        </td>
                        <td>
                            <asp:TextBox ID="txtTechnicalSpec" runat="server" CssClass="readonlytextbox" Width="360px" TextMode="MultiLine" Rows="5" ></asp:TextBox>
                        </td>
                    </tr>--%>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLocation" runat="server" Text="Location"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtLocation" runat="server" Width="160px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <telerik:RadLabel ID="lblJobDescription" runat="server" Text="Standard Job Description"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtJobDescription" runat="server" CssClass="input_mandatory" Width="100%"
                            TextMode="MultiLine" Rows="6">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr valign="top" runat="server" id="trVesselJobDesc">
                    <td>
                        <telerik:RadLabel ID="lblVesselJobDescription" runat="server" Text="Vessel Job Description"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtVesselJobDescription" runat="server" CssClass="input_mandatory" Width="100%"
                            TextMode="MultiLine" Rows="6">
                        </telerik:RadTextBox>
                        <telerik:RadLabel ID="lblTempVesselJobDescription" runat="server" Visible="false"></telerik:RadLabel>
                    </td>
                </tr>
                <tr valign="top" runat="server" id="trOfficeJobDesc">
                    <td>
                        <telerik:RadLabel ID="ltrlOfficeJobDescription" runat="server" Text="Office Job Description"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtOfficeJobDescription" runat="server" CssClass="input_mandatory" Width="100%"
                            TextMode="MultiLine" Rows="6">
                        </telerik:RadTextBox>
                        <telerik:RadLabel ID="lblTempOfficeJobDescription" runat="server" Visible="false"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>

                    <td>
                        <telerik:RadLabel ID="lblResponsibilty" runat="server" Text="Responsibilty"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadListBox CheckBoxes="true" ID="cblResponsibilty" runat="server" RepeatDirection="Horizontal">
                        </telerik:RadListBox>
                    </td>
                </tr>
                <tr runat="server" visible="false">
                    <td>
                        <telerik:RadLabel ID="lblEstimatedDurationHrs" runat="server" Text="Estimated Duration(Hrs)"></telerik:RadLabel>

                    </td>
                    <td>
                        <eluc:Number runat="server" ID="txtDuration"  Width="60px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCostClassification" runat="server" Text="Cost Classification"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlCostClassification" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" DefaultMessage="--Select--">
                        </telerik:RadDropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblWorktobesurveyedby" runat="server" Text="Work to be surveyed by"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadListBox CheckBoxes="true" ID="cblWorkSurvey" runat="server" RepeatDirection="Horizontal">
                        </telerik:RadListBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMaterial" runat="server" Text="Material"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadListBox CheckBoxes="true" ID="cblMaterial" runat="server" RepeatDirection="Horizontal">
                        </telerik:RadListBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEnclosed" runat="server" Text="Enclosed"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadListBox CheckBoxes="true" ID="cblEnclosed" runat="server" RepeatDirection="Horizontal">
                        </telerik:RadListBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEstimatedDays" runat="server" Text="Estimated Days"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblEstimatedDays" runat="server" RepeatDirection="Horizontal">
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                 <tr>
                        <td>
                            <telerik:RadLabel ID="lblbudgetcode" runat="server" Text="Budgetcode"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox runat="server" ID="radddlbudgetcode"  AllowCustomText
                                ="true" EmptyMessage="Type to select Budgetcode" Width="180px"/>
                        </td>
                    </tr>
            </table>
            <hr />
            <%--   <asp:GridView ID="gvComponent" runat="server" AutoGenerateColumns="False"
                Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvComponent_RowCommand"
                OnRowDataBound="gvComponent_RowDataBound" OnRowCancelingEdit="gvComponent_RowCancelingEdit"
                OnRowDeleting="gvComponent_RowDeleting" OnRowEditing="gvComponent_RowEditing"
                ShowFooter="true" ShowHeader="true" EnableViewState="false" OnRowUpdating="gvComponent_RowUpdating" DataKeyNames="FLDDTKEY">
                <FooterStyle CssClass="datagrid_FooterStyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                <RowStyle Height="10px" />
                <Columns>--%>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvComponent" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None"
                OnNeedDataSource="gvComponent_NeedDataSource"
                OnItemDataBound="gvComponent_ItemDataBound"
                OnItemCommand="gvComponent_ItemCommand"
                OnUpdateCommand="gvComponent_UpdateCommand"     EnableHeaderContextMenu="true" GroupingEnabled="false" >
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false" ></SortingSettings>

                <MasterTableView EditMode="InPlace" ShowFooter="true" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDDTKEY" TableLayout="Fixed" >
                  <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="RadLabel3" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>

                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Component">
                         <HeaderStyle Width="40%" Wrap="true" HorizontalAlign="Left"    />
                               <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblComponentid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></telerik:RadLabel>
                                <%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNUMBER")%> - <%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListComponent">
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtComponentCode" runat="server" CssClass="input_mandatory" MaxLength="20"
                                        Enabled="false" Width="60px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>'>
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtComponentName" runat="server" CssClass="input_mandatory" MaxLength="20"
                                        Enabled="false" Width="210px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'>
                                    </telerik:RadTextBox>
                                    <asp:LinkButton ID="imgComponent" runat="server"  ><span class="icon"><i class="fas fa-tasks"></i></span></asp:LinkButton>

                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtComponentId" runat="server" CssClass="hidden" Width="0px" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTID")%>'></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <span id="spnPickListComponentAdd">
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtComponentCodeAdd" runat="server" CssClass="input_mandatory" MaxLength="20"
                                        Enabled="false" Width="60px">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtComponentNameAdd" runat="server" CssClass="input_mandatory" MaxLength="20"
                                        Enabled="false" Width="210px">
                                    </telerik:RadTextBox>
                                    <asp:LinkButton ID="imgComponentAdd" runat="server" >   <span class="icon"><i class="fas fa-tasks"></i></span></asp:LinkButton>
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtComponentIdAdd" runat="server" CssClass="hidden" Width="0px"></telerik:RadTextBox>
                                </span>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Specification">
                         <HeaderStyle Width="50%" Wrap="true" HorizontalAlign="Left"    />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDescription" runat="server"></telerik:RadLabel>
                                <eluc:ToolTip runat="server" ID="ucTooltipDesc" Text='' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox RenderMode="Lightweight"  Width ="100%"     ID="txtDescription" runat="server" CssClass="gridinput" Text='<%# General.SanitizeHtml(DataBinder.Eval(Container, "DataItem.FLDSPECIFICATION").ToString())%>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                          
                            <ItemStyle Wrap="False" HorizontalAlign="Center" ></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCompEdit"
                                    ToolTip="Edit" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" Width="3" />
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCompDelete"
                                    ToolTip="Delete" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCompSave"
                                    ToolTip="Save" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" Width="3" />
                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCompCancel"
                                    ToolTip="Cancel" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-trash-alt"></i></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCompAdd"
                                    ToolTip="Add New" Width="20PX" Height="20PX">
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
                    <Scrolling AllowScroll="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <hr />
            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuDryDockStandardJobLineItem" runat="server" OnTabStripCommand="DryDockStandardJobLineItem_TabStripCommand"></eluc:TabStrip>
            </div>
            <div id="divGrid" style="position: relative; z-index: +1;">
                <%--<asp:GridView ID="gvStandardJobLineItem" runat="server" AutoGenerateColumns="False"
                    Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvStandardJobLineItem_RowCommand"
                    OnRowDataBound="gvStandardJobLineItem_ItemDataBound" OnRowCancelingEdit="gvStandardJobLineItem_RowCancelingEdit"
                    OnRowDeleting="gvStandardJobLineItem_RowDeleting" OnRowEditing="gvStandardJobLineItem_RowEditing"
                    ShowFooter="true" ShowHeader="true" EnableViewState="false" OnSorting="gvStandardJobLineItem_Sorting"
                    AllowSorting="true" OnRowCreated="gvStandardJobLineItem_RowCreated" OnSelectedIndexChanging="gvStandardJobLineItem_SelectedIndexChanging"
                    OnRowUpdating="gvStandardJobLineItem_RowUpdating">
                    <FooterStyle CssClass="datagrid_FooterStyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="10px" />
                    <Columns>--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvStandardJobLineItem" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None"
                    OnNeedDataSource="gvStandardJobLineItem_NeedDataSource"
                    OnItemCommand="gvStandardJobLineItem_ItemCommand"
                    OnItemDataBound="gvStandardJobLineItem_ItemDataBound1"
                    OnUpdateCommand="gvStandardJobLineItem_UpdateCommand"   GroupingEnabled="false" EnableHeaderContextMenu="true"  >
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                    <MasterTableView ShowFooter="true" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDJOBDETAILID" TableLayout="Fixed"  >
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
                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Include">
                                <ItemStyle Wrap="true" HorizontalAlign="Center" Width="5%"></ItemStyle>
                                 <HeaderStyle Wrap="true" HorizontalAlign="Left" Width="15%" />
                                <ItemTemplate>
                                    <telerik:RadCheckBox runat="server" ID="chkSelectedYN" Text="" BackColor="Transparent" />
                                    <telerik:RadButton runat="server" ID="cmdSelectedYN" Visible="true" Text="<%# Container.DataSetIndex %>"
                                        CommandName="SELECTJOB" CommandArgument="<%# Container.DataSetIndex %>" Width="0px" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Job Detail">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                             <HeaderStyle Wrap="true" HorizontalAlign="Left" Width="36%" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lbljobdetailid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBDETAILID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblDetail" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lbljobdetailidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBDETAILID") %>'></telerik:RadLabel>
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtDetailEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'
                                        CssClass="gridinput_mandatory" ToolTip="Job Detail"  Width="100%">
                                    </telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtDetailAdd" runat="server" CssClass="gridinput_mandatory"
                                        ToolTip="Job Detail"    Width="100%">
                                    </telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Unit">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                  <HeaderStyle Wrap="true" HorizontalAlign="Left" Width="15%" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblUnit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Unit ID="ucUnitEdit" runat="server" AppendDataBoundItems="true" UnitList='<%# PhoenixRegistersUnit.ListUnit()%>'
                                        SelectedUnit='<%# DataBinder.Eval(Container,"DataItem.FLDUNIT") %>'  Width="100%"/>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Unit ID="ucUnitAdd"  runat="server" AppendDataBoundItems="true" UnitList='<%# PhoenixRegistersUnit.ListUnit()%>'  Width="100%"   />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Edit"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                        ToolTip="Edit">
                                    <span class="icon"><i class="fa fa-edit"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Move Up"
                                        CommandName="MOVEUP" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdMoveUp"
                                        ToolTip="Move Up">
                                    
                                    <span class="icon"><i class="fas fa-angle-double-up"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Move Down"
                                        CommandName="MOVEDOWN" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdMoveDown"
                                        ToolTip="Move Down">
                                     <span class="icon"><i class="fas fa-angle-double-down"></i></span>
                                    </asp:LinkButton>
                                    <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" Width="3" />
                                    <asp:LinkButton runat="server" AlternateText="Delete"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                        ToolTip="Delete" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Save"
                                        CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                        ToolTip="Save" Width="20px" Height="20px">
                                     <span class="icon"><i class="fa fa-save"></i></span>
                                    </asp:LinkButton>
                                    <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" Width="3" />
                                    <asp:LinkButton runat="server" AlternateText="Cancel"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                    </asp:LinkButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Save"
                                        CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                        ToolTip="Add New" Width="20px" Height="20px">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                    </asp:LinkButton>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced"  PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </div>
            <br />
            <b>Additional Jobs</b>
            <%-- <asp:GridView ID="gvAddDetail" runat="server" AutoGenerateColumns="False"
                Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvAddDetail_RowCommand"
                OnRowDataBound="gvAddDetail_RowDataBound" OnRowCancelingEdit="gvAddDetail_RowCancelingEdit"
                OnRowDeleting="gvAddDetail_RowDeleting" OnRowEditing="gvAddDetail_RowEditing"
                ShowFooter="true" ShowHeader="true" EnableViewState="false"
                OnRowUpdating="gvAddDetail_RowUpdating" DataKeyNames="FLDADDITIONALJOBID">
                <FooterStyle CssClass="datagrid_FooterStyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                <RowStyle Height="10px" />
                <Columns>--%>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvAddDetail" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None"
               
                OnNeedDataSource="gvAddDetail_NeedDataSource"
                OnItemDataBound ="gvAddDetail_ItemDataBound"
                OnItemCommand="gvAddDetail_ItemCommand"
                OnUpdateCommand="gvAddDetail_UpdateCommand" EnableHeaderContextMenu="true"  GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                <MasterTableView ShowFooter="true" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDADDITIONALJOBID" TableLayout="Fixed" Height="10px">
                   
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="RadLabel2" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                     <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                    <Columns>

                        <telerik:GridTemplateColumn HeaderText="Project">
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="10%"></ItemStyle>
                           <HeaderStyle Wrap="true" HorizontalAlign="Left" Width="20%" />
                            <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>
                        </ItemTemplate>
                            <EditItemTemplate>
                            <telerik:RadDropDownList  Width ="100%"     RenderMode="Lightweight" ID="ddlProjectEdit" runat="server" CssClass="input_mandatory"></telerik:RadDropDownList>
                        </EditItemTemplate>
                            <FooterTemplate>
                            <telerik:RadDropDownList RenderMode="Lightweight"  Width ="100%" 
                                DataTextField="FLDNUMBER"
                                DataValueField="FLDORDERID"
                                ID="ddlProjectAdd" runat="server" CssClass="input_mandatory" EnableDirectionDetection="true"></telerik:RadDropDownList>
                        </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Job Detail">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70%"></ItemStyle>
                           <HeaderStyle Wrap="true" HorizontalAlign="Left" Width="40%" />
                            <ItemTemplate>
                            <telerik:RadLabel ID="lblDetail" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                        </ItemTemplate>
                            <EditItemTemplate>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtDetailEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'
                                CssClass="gridinput_mandatory"  Width ="100%"   ToolTip="Job Detail"></telerik:RadTextBox>
                        </EditItemTemplate>
                            <FooterTemplate>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtDetailAdd" runat="server" CssClass="gridinput_mandatory"     Width ="100%" 
                                ToolTip="Job Detail"></telerik:RadTextBox>
                        </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Unit">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Wrap="true" HorizontalAlign="Left" Width="30%" />
                            <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>
                        </ItemTemplate>
                            <EditItemTemplate>
                            <eluc:Unit ID="ucUnitEdit" runat="server" AppendDataBoundItems="true" UnitList='<%# PhoenixRegistersUnit.ListUnit()%>'
                                Width ="100%"  SelectedUnit='<%# DataBinder.Eval(Container,"DataItem.FLDUNIT") %>' />
                        </EditItemTemplate>
                            <FooterTemplate>
                            <eluc:Unit  Width ="100%"   ID="ucUnitAdd" runat="server" AppendDataBoundItems="true" UnitList='<%# PhoenixRegistersUnit.ListUnit()%>' />
                        </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" verticalalign="Middle"></HeaderStyle>
                            
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                            <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Edit" 
                                CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                ToolTip="Edit">
                                <span class="icon"><i class="fa fa-edit"></i></span>
                            </asp:LinkButton>
                            <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" Width="3" />
                            <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Delete" 
                                CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                ToolTip="Delete">
                                <span class="icon"><i class="fas fa-trash-alt"></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>
                            <EditItemTemplate>
                            <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save"
                                CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                ToolTip="Save">
                                <span class="icon"><i class="fa fa-save"></i></span>
                            </asp:LinkButton>
                            <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" Width="3" />
                            <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Cancel" 
                                CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                ToolTip="Cancel">
                                <span class="icon"><i class="fas fa-trash-alt"></i></span>
                            </asp:LinkButton>
                        </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                            <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save" 
                                CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
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
                    <Scrolling AllowScroll="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status ID="ucStatus" runat="server" />
                 </telerik:RadAjaxPanel>
        </div>
       
    </form>
</body>
</html>