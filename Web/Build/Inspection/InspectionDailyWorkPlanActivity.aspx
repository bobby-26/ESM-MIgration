<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDailyWorkPlanActivity.aspx.cs" Inherits="InspectionDailyWorkPlanActivity" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VerticalSplit" Src="~/UserControls/UserControlVerticalSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="WorkPermit" Src="~/UserControls/UserControlWorkPermit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskedTextBox.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByCompany" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="WorkPermitByCompany" Src="~/UserControls/UserControlWorkPermitByCompany.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Search Results</title>
    <telerik:RadCodeBlock ID="divHead1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </telerik:RadCodeBlock>

    <script language="javascript" type="text/javascript">
        function SelectSingleRadiobutton(rdbtnid) {
            var rdBtn = document.getElementById(rdbtnid);
            var rdBtnList = document.getElementsByTagName("input");
            for (i = 0; i < rdBtnList.length; i++) {
                if (rdBtnList[i].type == "radio" && rdBtnList[i].id != rdBtn.id) {
                    rdBtnList[i].checked = false;
                }
            }
        }

    </script>
</head>
<body>
    <form id="frmSearchResults" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="panel" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <telerik:RadButton runat="server" ID="cmdHiddenSectionmit" OnClick="cmdHiddenSectionmit_Click" />
                <div style="font-weight: 600; font-size: 12px;" runat="server">
                    <eluc:TabStrip ID="MenuDeficiencyGeneral" runat="server" Title="Work Plan Activity" OnTabStripCommand="DeficiencyGeneral_TabStripCommand"></eluc:TabStrip>
                </div>

                <%--<div class="subHeader" style="position: relative;">
                    <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                        <eluc:TabStrip ID="WorkPlanMain" runat="server" OnTabStripCommand="WorkPlanMain_TabStripCommand">
                        </eluc:TabStrip>
                    </span>
                </div>--%>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblWorkPlan" width="100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:VesselByCompany runat="server" ID="ucVessel" AppendDataBoundItems="true" CssClass="input_mandatory"
                                    VesselsOnly="true" AutoPostBack="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="ucDate" runat="server" CssClass="input_mandatory" DatePicker="true" DateTimeFormat="dd/MM/YYYY" />
                            </td>
                        </tr>
                    </table>
                </div>
                <telerik:RadLabel ID="lblOperations" runat="server" Text="Operations" Style="font-weight: bold"></telerik:RadLabel>

                <%--  <asp:GridView ID="gvOperations" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvOperations_RowCommand" OnRowDataBound="gvOperations_ItemDataBound"
                        OnRowCreated="gvOperations_RowCreated" ShowFooter="true" ShowHeader="true"
                        EnableViewState="false" AllowSorting="true" OnSelectedIndexChanging="gvOperations_SelectedIndexChanging"                        
                        OnPreRender="gvOperations_PreRender" OnSorting="gvOperations_Sorting" DataKeyNames="FLDWORKPLANACTIVITYID">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>--%>

                <telerik:RadGrid RenderMode="Lightweight" ID="gvOperations" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" EnableViewState="false"
                    GroupingEnabled="false" EnableHeaderContextMenu="true"
                    OnItemCommand="gvOperations_ItemCommand"
                    OnNeedDataSource="gvOperations_NeedDataSource"
                    OnItemDataBound="gvOperations_ItemDataBound1">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed" ShowFooter="true">
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
                            <telerik:GridButtonColumn Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <telerik:GridTemplateColumn HeaderText="">
                                <HeaderStyle Width="30px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Edit"
                                        CommandName="OPERATIONSCLEAR" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdClear"
                                        ToolTip="Clear Selection">
                                        <span class="icon"><i class="fas fa-eraser"></i></span>
                                    </asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:RadioButton ID="rdbUser" runat="server" OnClick="javascript:SelectSingleRadiobutton(this.id)" />
                                    <telerik:RadLabel ID="lblWorkPlanActivityId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKPLANACTIVITYID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <telerik:RadLabel ID="lblWorkPlanActivityLineItemId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKPLANACTIVITYLINEITEMID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="S.No">
                                <HeaderStyle Width="60px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>'></telerik:RadLabel>

                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblWorkPlanActivityIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKPLANACTIVITYID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <telerik:RadLabel ID="lblWorkPlanActivityLineItemIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKPLANACTIVITYLINEITEMID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <eluc:Number ID="ucSNoEdit" runat="server" Width="100%"
                                        MaskText="###" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number ID="ucSNoAdd" runat="server" Width="100%"
                                        MaskText="###" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Activity">
                                <HeaderStyle Width="150px" />

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblActivity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITY") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox ID="txtActivityEdit" runat="server" CssClass="input_mandatory" MaxLength="200" Width="100%"
                                        TextMode="MultiLine" Rows="2" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITY") %>'>
                                    </telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtActivityAdd" runat="server" CssClass="input_mandatory" MaxLength="200"
                                        TextMode="MultiLine" Rows="2" Width="100%">
                                    </telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Person Incharge">
                                <HeaderStyle Width="150px" />

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblIncharge" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICNAMERANK") %>'></telerik:RadLabel>
                                    <eluc:ToolTip ID="ucToolTipPICName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICNAMERANK") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <span id="spnCrewInChargeEdit">
                                        <telerik:RadTextBox ID="txtCrewNameEdit" runat="server"  Enabled="false"
                                            MaxLength="50" Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCHARGENAME") %>'>
                                        </telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtCrewRankEdit" runat="server"  Enabled="false"
                                            MaxLength="50" Width="50px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCHARGERANK") %>'>
                                        </telerik:RadTextBox>
                                        <asp:LinkButton ID="imgShowCrewInChargeEdit" runat="server"
                                            ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>">
                                            <span class="icon"><i class="fas fa-tasks"></i></span>
                                        </asp:LinkButton>
                                        <telerik:RadTextBox ID="txtCrewIdEdit" runat="server"  MaxLength="0" Width="0px"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCHARGE") %>'>
                                        </telerik:RadTextBox>
                                    </span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <span id="spnCrewInChargeAdd">
                                        <telerik:RadTextBox ID="txtCrewNameAdd" runat="server"  Enabled="false"
                                            MaxLength="50" Width="80px">
                                        </telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtCrewRankAdd" runat="server"  Enabled="false"
                                            MaxLength="50" Width="50px">
                                        </telerik:RadTextBox>
                                        <asp:LinkButton ID="imgShowCrewInChargeAdd" runat="server"
                                            ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>">
                                              <span class="icon"><i class="fas fa-tasks"></i></span>
                                        </asp:LinkButton>
                                        <telerik:RadTextBox ID="txtCrewIdAdd" runat="server"  MaxLength="20" Width="0px"></telerik:RadTextBox>
                                    </span>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Job Hazard Analysis">
                                <HeaderStyle Width="150px" />

                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkHazardNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNUMBER") %>'></asp:LinkButton>
                                    <telerik:RadLabel ID="lblHazardNumber" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNUMBERNA") %>' Width="170px"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblHazardId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBHAZARDID") %>' Visible="false"></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <span id="spnHazardEdit">
                                        <telerik:RadTextBox ID="txtHazardNumberEdit" runat="server"  Enabled="false"
                                            MaxLength="50" Width="50px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNUMBER") %>'>
                                        </telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtHazardEdit" runat="server"  Enabled="false" MaxLength="50"
                                            Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOB") %>'>
                                        </telerik:RadTextBox>
                                        <asp:LinkButton ID="imgShowHazardEdit" runat="server"
                                            ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" >
                                              <span class="icon"><i class="fas fa-tasks"></i></span>
                                        </asp:LinkButton>
                                        <telerik:RadTextBox ID="txtHazardIdEdit" runat="server"  Width="0px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBHAZARDID") %>'></telerik:RadTextBox>
                                    </span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <span id="spnHazardAdd">
                                        <telerik:RadTextBox ID="txtHazardNumberAdd" runat="server"  Enabled="false"
                                            MaxLength="50" Width="50px">
                                        </telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtHazardAdd" runat="server"  Enabled="false" MaxLength="50"
                                            Width="80px">
                                        </telerik:RadTextBox>
                                        <asp:LinkButton ID="imgShowHazardAdd" runat="server"
                                            ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" >
                                              <span class="icon"><i class="fas fa-tasks"></i></span>
                                        </asp:LinkButton>
                                        <telerik:RadTextBox ID="txtHazardIdAdd" runat="server"  Width="0px"></telerik:RadTextBox>
                                    </span>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Work Permit">
                                <HeaderStyle Width="150px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkWorkPermit" runat="server" Target="_blank"
                                        Style="text-decoration: underline; cursor: pointer; color: Blue;" Text='<%# DataBinder.Eval(Container, "DataItem.FLDWORKPERMIT").ToString().Length > 20 ? DataBinder.Eval(Container, "DataItem.FLDWORKPERMIT").ToString().Substring(0, 20) : DataBinder.Eval(Container, "DataItem.FLDWORKPERMIT").ToString() %>'>
                                    </asp:HyperLink>
                                    <telerik:RadLabel ID="lblWorkPermit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDWORKPERMITNA").ToString().Length > 20 ? DataBinder.Eval(Container, "DataItem.FLDWORKPERMITNA").ToString().Substring(0, 20) : DataBinder.Eval(Container, "DataItem.FLDWORKPERMITNA").ToString() %>'></telerik:RadLabel>
                                    <eluc:ToolTip ID="ucToolTipWorkPermit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKPERMIT") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblWorkPermitIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKPERMITID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <eluc:WorkPermitByCompany runat="server" ID="ucWorkPermitEdit" AppendDataBoundItems="true" Width="100%"
                                         CategoryNumber="3" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:WorkPermitByCompany runat="server" ID="ucWorkPermitAdd" AppendDataBoundItems="true" Width="100%"
                                         CategoryNumber="3" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Risk Assessment">
                                <HeaderStyle Width="150px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRAId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENTID") %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkRA" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRAREFNO") %>' Width="175px"></asp:LinkButton>
                                    <telerik:RadLabel ID="lblRA" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRAREFNONA") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblOperationJHAList" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMPORTEDJHA") %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="imgOperationJHAList" AlternateText="Show Imported JHA"
                                        runat="server" ToolTip="Show Imported JHA" >
                                          <span class="icon"><i class="fas fa-clipboard-list"></i></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <span id="spnRAEdit">
                                        <telerik:RadTextBox ID="txtRANumberEdit" runat="server"  Enabled="false"
                                            MaxLength="50" Width="50px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRAREFNO") %>'>
                                        </telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtRAEdit" runat="server"  Enabled="false" MaxLength="50"
                                            Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENT") %>'>
                                        </telerik:RadTextBox>
                                        <asp:LinkButton ID="imgShowRAEdit" runat="server"
                                            ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" >
                                              <span class="icon"><i class="fas fa-tasks"></i></span>
                                        </asp:LinkButton>
                                        <telerik:RadTextBox ID="txtRAIdEdit" runat="server"  MaxLength="20" Width="0px"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENTID") %>'>
                                        </telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtRaTypeEdit" runat="server"  MaxLength="2" Width="0px"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENTTYPE") %>'>
                                        </telerik:RadTextBox>
                                    </span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <span id="spnRAAdd">
                                        <telerik:RadTextBox ID="txtRANumberAdd" runat="server"  Enabled="false"
                                            MaxLength="50" Width="50px">
                                        </telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtRAAdd" runat="server"  Enabled="false" MaxLength="50"
                                            Width="80px">
                                        </telerik:RadTextBox>
                                        <asp:LinkButton ID="imgShowRAAdd" runat="server" 
                                            ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" >
                                              <span class="icon"><i class="fas fa-tasks"></i></span>
                                        </asp:LinkButton>
                                        <telerik:RadTextBox ID="txtRAIdAdd" runat="server"  MaxLength="20" Width="0px"></telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtRaTypeAdd" runat="server"  MaxLength="2" Width="0px"></telerik:RadTextBox>
                                    </span>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Est Start Time">
                                <HeaderStyle Width="80px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                <FooterStyle VerticalAlign="Middle" />

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblStartTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTARTDATETIME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number MaskText="##:##" ID="txtStartTimeEdit" runat="server"  Width="100%" />
                                    <%-- <ajaxToolkit:MaskedEditExtender ID="MaskedtxtStartTimeEdit" runat="server" AcceptAMPM="false"
                                        ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                                        TargetControlID="txtStartTimeEdit" UserTimeFormat="TwentyFourHour" />
                                    <telerik:RadLabel ID="lblHrs" runat="server" Text="hrs"></telerik:RadLabel>--%>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number MaskText="##:##" ID="txtStartTimeAdd" runat="server"  Width="100%" />

                                    <telerik:RadLabel ID="lblHrs1" runat="server" Text="hrs"></telerik:RadLabel>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Est End Time">
                                <HeaderStyle Width="80px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                <FooterStyle VerticalAlign="Middle" />

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblEndTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENDDATETIME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number MaskText="##:##" ID="txtEndTimeEdit" runat="server"  Width="100%" />

                                    hrs
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number MaskText="##:##" ID="txtEndTimeAdd" runat="server"  Width="100%" />

                                    hrs
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">

                                <ItemStyle Wrap="false" HorizontalAlign="Left" />

                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="40px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Edit"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                        ToolTip="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>

                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Delete"
                                        CommandName="OPERATIONSDELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                        ToolTip="Delete">
                                            <span class="icon"><i class="fa fa-trash"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save"
                                        CommandName="UPDATE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                        ToolTip="Save">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                                    </asp:LinkButton>
                                    <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Cancel"
                                        CommandName="CANCEL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel">
                                            <span class="icon"><i class="fas fa-times-circle"></i></span>
                                    </asp:LinkButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save"
                                        CommandName="OPERATIONSADD" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
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
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>



                <br />
                <telerik:RadLabel ID="lblDeck" runat="server" Text="Deck" Style="font-weight: bold"></telerik:RadLabel>

                <%--<asp:GridView ID="gvDeck" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="gvDeck_RowDataBound" OnRowCommand="gvDeck_RowCommand"
                        Style="margin-bottom: 0px" OnSelectedIndexChanging="gvDeck_SelectedIndexChanging"
                        OnPreRender="gvDeck_PreRender" EnableViewState="false" ShowFooter="true"
                        DataKeyNames="FLDWORKPLANACTIVITYID">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvDeck" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" EnableViewState="false"
                    GroupingEnabled="false" EnableHeaderContextMenu="true"
                    OnItemCommand="gvDeck_ItemCommand"
                    OnItemDataBound="gvDeck_ItemDataBound"
                    OnNeedDataSource="gvDeck_NeedDataSource">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed" ShowFooter="true">
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
                            <telerik:GridButtonColumn Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <telerik:GridTemplateColumn>
                                <HeaderStyle Width="30px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Edit" 
                                        CommandName="DECKCLEAR" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdClear"
                                        ToolTip="Clear Selection">
                                        <span class="icon"><i class="fas fa-eraser"></i></span>
                                    </asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:RadioButton ID="rdbUser" runat="server" OnClick="javascript:SelectSingleRadiobutton(this.id)" AutoPostBack="true" />
                                    <telerik:RadLabel ID="lblWorkPlanActivityId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKPLANACTIVITYID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <telerik:RadLabel ID="lblWorkPlanActivityLineItemId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKPLANACTIVITYLINEITEMID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="S.No">
                                <HeaderStyle Width="60px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>'></telerik:RadLabel>

                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblWorkPlanActivityIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKPLANACTIVITYID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <telerik:RadLabel ID="lblWorkPlanActivityLineItemIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKPLANACTIVITYLINEITEMID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <eluc:Number ID="ucSNoEdit" runat="server" Width="100%"
                                        MaskText="###" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number ID="ucSNoAdd" runat="server" Width="100%"
                                        MaskText="###" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Activity">
                                <HeaderStyle Width="150px" />
                                <ItemStyle Wrap="true" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblActivity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITY") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox ID="txtActivityEdit" runat="server" CssClass="input_mandatory" MaxLength="200" Width="100%"
                                        TextMode="MultiLine" Rows="2" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITY") %>'>
                                    </telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtActivityAdd" runat="server" CssClass="input_mandatory" MaxLength="200"
                                        TextMode="MultiLine" Rows="2" Width="100%">
                                    </telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Person Incharge">
                                <HeaderStyle Width="150px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblIncharge" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICNAMERANK") %>'></telerik:RadLabel>
                                    <eluc:ToolTip ID="ucToolTipPICName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICNAMERANK") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <span id="spnCrewInChargeEditSection">
                                        <telerik:RadTextBox ID="txtCrewNameEdit" runat="server"  Enabled="false"
                                            MaxLength="50" Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCHARGENAME") %>'>
                                        </telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtCrewRankEdit" runat="server"  Enabled="false"
                                            MaxLength="50" Width="50px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCHARGERANK") %>'>
                                        </telerik:RadTextBox>
                                        <asp:LinkButton ID="imgShowCrewInChargeEdit" runat="server" 
                                            ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" >
                                              <span class="icon"><i class="fas fa-tasks"></i></span>
                                        </asp:LinkButton>
                                        <telerik:RadTextBox ID="txtCrewIdEdit" runat="server"  MaxLength="0" Width="0px"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCHARGE") %>'>
                                        </telerik:RadTextBox>
                                    </span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <span id="spnCrewInChargeAddSection">
                                        <telerik:RadTextBox ID="txtCrewNameAdd" runat="server"  Enabled="false"
                                            MaxLength="50" Width="80px">
                                        </telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtCrewRankAdd" runat="server"  Enabled="false"
                                            MaxLength="50" Width="50px">
                                        </telerik:RadTextBox>
                                        <asp:LinkButton ID="imgShowCrewInChargeAdd" runat="server" 
                                            ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" >
                                              <span class="icon"><i class="fas fa-tasks"></i></span>
                                        </asp:LinkButton>
                                        <telerik:RadTextBox ID="txtCrewIdAdd" runat="server"  MaxLength="20" Width="0px"></telerik:RadTextBox>
                                    </span>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Job Hazard Analysis">
                                <HeaderStyle Width="150px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkHazardNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNUMBER") %>'></asp:LinkButton>
                                    <telerik:RadLabel ID="lblHazardNumber" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNUMBERNA") %>' Width="170px"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblHazardId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBHAZARDID") %>' Visible="false"></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <span id="spnHazardEditSection">
                                        <telerik:RadTextBox ID="txtHazardNumberEdit" runat="server"  Enabled="false"
                                            MaxLength="50" Width="50px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNUMBER") %>'>
                                        </telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtHazardEdit" runat="server"  Enabled="false" MaxLength="50"
                                            Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOB") %>'>
                                        </telerik:RadTextBox>
                                        <asp:LinkButton ID="imgShowHazardEdit" runat="server" 
                                            ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" >
                                              <span class="icon"><i class="fas fa-tasks"></i></span>
                                        </asp:LinkButton>
                                        <telerik:RadTextBox ID="txtHazardIdEdit" runat="server"  Width="0px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBHAZARDID") %>'></telerik:RadTextBox>
                                    </span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <span id="spnHazardAddSection">
                                        <telerik:RadTextBox ID="txtHazardNumberAdd" runat="server"  Enabled="false"
                                            MaxLength="50" Width="50px">
                                        </telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtHazardAdd" runat="server"  Enabled="false" MaxLength="50"
                                            Width="80px">
                                        </telerik:RadTextBox>
                                        <asp:LinkButton ID="imgShowHazardAdd" runat="server" 
                                            ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" >
                                              <span class="icon"><i class="fas fa-tasks"></i></span>
                                        </asp:LinkButton>
                                        <telerik:RadTextBox ID="txtHazardIdAdd" runat="server"  Width="0px"></telerik:RadTextBox>
                                    </span>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Work Permit">
                                <HeaderStyle Width="150px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkWorkPermit" runat="server" Target="_blank"
                                        Style="text-decoration: underline; cursor: pointer; color: Blue;" Text='<%# DataBinder.Eval(Container, "DataItem.FLDWORKPERMIT").ToString().Length > 20 ? DataBinder.Eval(Container, "DataItem.FLDWORKPERMIT").ToString().Substring(0, 20) : DataBinder.Eval(Container, "DataItem.FLDWORKPERMIT").ToString() %>'>
                                    </asp:HyperLink>
                                    <telerik:RadLabel ID="lblWorkPermit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDWORKPERMITNA").ToString().Length > 20 ? DataBinder.Eval(Container, "DataItem.FLDWORKPERMITNA").ToString().Substring(0, 20) : DataBinder.Eval(Container, "DataItem.FLDWORKPERMITNA").ToString() %>'></telerik:RadLabel>
                                    <eluc:ToolTip ID="ucToolTipWorkPermit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKPERMIT") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblWorkPermitIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKPERMITID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <eluc:WorkPermitByCompany runat="server" ID="ucWorkPermitEdit" AppendDataBoundItems="true"
                                         CategoryNumber="3" Width="100%" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:WorkPermitByCompany runat="server" ID="ucWorkPermitAdd" AppendDataBoundItems="true"
                                         CategoryNumber="3" Width="100%" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Risk Assessment">
                                <HeaderStyle Width="150px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRAId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENTID") %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkRA" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRAREFNO") %>'></asp:LinkButton>
                                    <telerik:RadLabel ID="lblRA" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRAREFNONA") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblDeckJHAList" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMPORTEDJHA") %>'></telerik:RadLabel>
                                    <asp:LinkButton  ID="imgDeckJHAList" AlternateText="Show Imported JHA"
                                        runat="server" ToolTip="Show Imported JHA" >
                                        <span class="icon"><i class="fas fa-clipboard-list"></i></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <span id="spnRAEditSection">
                                        <telerik:RadTextBox ID="txtRANumberEdit" runat="server"  Enabled="false"
                                            MaxLength="50" Width="50px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRAREFNO") %>'>
                                        </telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtRAEdit" runat="server"  Enabled="false" MaxLength="50"
                                            Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENT") %>'>
                                        </telerik:RadTextBox>
                                        <asp:LinkButton ID="imgShowRAEdit" runat="server" 
                                            ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" >
                                              <span class="icon"><i class="fas fa-tasks"></i></span>
                                        </asp:LinkButton>
                                        <telerik:RadTextBox ID="txtRAIdEdit" runat="server"  MaxLength="20" Width="0px"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENTID") %>'>
                                        </telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtRaTypeEdit" runat="server"  MaxLength="2" Width="0px"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENTTYPE") %>'>
                                        </telerik:RadTextBox>
                                    </span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <span id="spnRAAddSection">
                                        <telerik:RadTextBox ID="txtRANumberAdd" runat="server"  Enabled="false"
                                            MaxLength="50" Width="50px">
                                        </telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtRAAdd" runat="server"  Enabled="false" MaxLength="50"
                                            Width="80px">
                                        </telerik:RadTextBox>
                                        <asp:LinkButton ID="imgShowRAAdd" runat="server"
                                            ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" >
                                              <span class="icon"><i class="fas fa-tasks"></i></span>
                                        </asp:LinkButton>
                                        <telerik:RadTextBox ID="txtRAIdAdd" runat="server"  MaxLength="20" Width="0px"></telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtRaTypeAdd" runat="server"  MaxLength="2" Width="0px"></telerik:RadTextBox>
                                    </span>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Est Start Time">
                                <HeaderStyle Width="80px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblStartTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTARTDATETIME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number MaskText="##:##" ID="txtStartTimeEdit" runat="server"  Width="100%" />

                                    hrs
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number MaskText="##:##" ID="txtStartTimeAdd" runat="server"  Width="100%" />

                                    hrs
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Est End Time">
                                <HeaderStyle Width="80px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblEndTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENDDATETIME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number MaskText="##:##" ID="txtEndTimeEdit" runat="server"  Width="100%" />

                                    hrs
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number MaskText="##:##" ID="txtEndTimeAdd" runat="server"  Width="100%" />

                                    hrs
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">

                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Edit"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                        ToolTip="Edit">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                    <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Delete"
                                        CommandName="DECKDELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                        ToolTip="Delete">
                                        <span class="icon"><i class="fa fa-trash"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save"
                                        CommandName="UPDATE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                        ToolTip="Save">
                                        <span class="icon"><i class="fas fa-save"></i></span>
                                    </asp:LinkButton>
                                    <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Cancel"
                                        CommandName="CANCEL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel">
                                         <span class="icon"><i class="fas fa-times-circle"></i></span>
                                    </asp:LinkButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save"
                                        CommandName="DECKADD" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
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
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>



                <br />
                <telerik:RadLabel ID="lblEngine" runat="server" Text="Engine" Style="font-weight: bold"></telerik:RadLabel>

                <%--  <asp:GridView ID="gvEngine" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="gvEngine_RowDataBound" OnRowCommand="gvEngine_RowCommand"
                        OnSelectedIndexChanging="gvEngine_SelectedIndexChanging" OnRowCreated="gvEngine_RowCreated"
                        OnPreRender="gvEngine_PreRender" ShowFooter="true" Style="margin-bottom: 0px"
                        EnableViewState="false" DataKeyNames="FLDWORKPLANACTIVITYID">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvEngine" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" EnableViewState="false"
                    GroupingEnabled="false" EnableHeaderContextMenu="true"
                    OnItemDataBound="gvEngine_ItemDataBound"
                    OnNeedDataSource="gvEngine_NeedDataSource"
                    OnItemCommand="gvEngine_ItemCommand">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed" ShowFooter="true">
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
                            <telerik:GridButtonColumn Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <telerik:GridTemplateColumn>
                                <HeaderStyle Width="30px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Edit"
                                        CommandName="ENGINECLEAR" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdClear"
                                        ToolTip="Clear Selection">
                                        <span class="icon"><i class="fas fa-eraser"></i></span>
                                    </asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:RadioButton ID="rdbUser" runat="server" OnClick="javascript:SelectSingleRadiobutton(this.id)" />
                                    <telerik:RadLabel ID="lblWorkPlanActivityId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKPLANACTIVITYID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <telerik:RadLabel ID="lblWorkPlanActivityLineItemId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKPLANACTIVITYLINEITEMID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="S.No">
                                <HeaderStyle Width="60px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>'></telerik:RadLabel>

                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblWorkPlanActivityIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKPLANACTIVITYID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <telerik:RadLabel ID="lblWorkPlanActivityLineItemIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKPLANACTIVITYLINEITEMID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <eluc:Number ID="ucSNoEdit" MaskText="###" CssClass="input_mandatory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>'
                                        Width="100%" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number ID="ucSNoAdd" MaskText="###" CssClass="input_mandatory" runat="server" Width="100%" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Activity">
                                <HeaderStyle Width="150px" />
                                <ItemStyle Wrap="true" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblActivity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITY") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox ID="txtActivityEdit" runat="server" CssClass="input_mandatory" MaxLength="200" Width="100%"
                                        TextMode="MultiLine" Rows="2" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITY") %>'>
                                    </telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtActivityAdd" runat="server" CssClass="input_mandatory" MaxLength="200"
                                        TextMode="MultiLine" Rows="2" Width="100%">
                                    </telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Person Incharge">
                                <HeaderStyle Width="150px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblIncharge" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICNAMERANK") %>'></telerik:RadLabel>
                                    <eluc:ToolTip ID="ucToolTipPICName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICNAMERANK") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <span id="spnCrewInChargeEditForm">
                                        <telerik:RadTextBox ID="txtCrewNameEdit" runat="server"  Enabled="false"
                                            MaxLength="50" Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCHARGENAME") %>'>
                                        </telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtCrewRankEdit" runat="server"  Enabled="false"
                                            MaxLength="50" Width="50px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCHARGERANK") %>'>
                                        </telerik:RadTextBox>
                                        <asp:LinkButton ID="imgShowCrewInChargeEdit" runat="server" 
                                            ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" >
                                              <span class="icon"><i class="fas fa-tasks"></i></span>
                                        </asp:LinkButton>

                                        <telerik:RadTextBox ID="txtCrewIdEdit" runat="server"  MaxLength="0" Width="0px"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCHARGE") %>'>
                                        </telerik:RadTextBox>
                                    </span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <span id="spnCrewInChargeAddForm">
                                        <telerik:RadTextBox ID="txtCrewNameAdd" runat="server"  Enabled="false"
                                            MaxLength="50" Width="80px">
                                        </telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtCrewRankAdd" runat="server"  Enabled="false"
                                            MaxLength="50" Width="50px">
                                        </telerik:RadTextBox>
                                        <asp:LinkButton ID="imgShowCrewInChargeAdd" runat="server"
                                            ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" >
                                              <span class="icon"><i class="fas fa-tasks"></i></span>
                                        </asp:LinkButton>
                                        <telerik:RadTextBox ID="txtCrewIdAdd" runat="server"  MaxLength="20" Width="0px"></telerik:RadTextBox>
                                    </span>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Job Hazard Analysis">
                                <HeaderStyle Width="150px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkHazardNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNUMBER") %>'></asp:LinkButton>
                                    <telerik:RadLabel ID="lblHazardNumber" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNUMBERNA") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblHazardId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBHAZARDID") %>' Visible="false"></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <span id="spnHazardEditForm">
                                        <telerik:RadTextBox ID="txtHazardNumberEdit" runat="server"  Enabled="false"
                                            MaxLength="50" Width="50px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNUMBER") %>'>
                                        </telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtHazardEdit" runat="server"  Enabled="false" MaxLength="50"
                                            Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOB") %>'>
                                        </telerik:RadTextBox>
                                        <asp:LinkButton ID="imgShowHazardEdit" runat="server" 
                                            ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" >
                                              <span class="icon"><i class="fas fa-tasks"></i></span>
                                        </asp:LinkButton>
                                        <telerik:RadTextBox ID="txtHazardIdEdit" runat="server"  Width="0px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBHAZARDID") %>'></telerik:RadTextBox>
                                    </span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <span id="spnHazardAddForm">
                                        <telerik:RadTextBox ID="txtHazardNumberAdd" runat="server"  Enabled="false"
                                            MaxLength="50" Width="50px">
                                        </telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtHazardAdd" runat="server"  Enabled="false" MaxLength="50"
                                            Width="80px">
                                        </telerik:RadTextBox>
                                        <asp:LinkButton ID="imgShowHazardAdd" runat="server"
                                            ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" >
                                              <span class="icon"><i class="fas fa-tasks"></i></span>
                                        </asp:LinkButton>
                                        <telerik:RadTextBox ID="txtHazardIdAdd" runat="server"  Width="0px"></telerik:RadTextBox>
                                    </span>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Work Permit">
                                <HeaderStyle Width="150px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkWorkPermit" runat="server" Target="_blank"
                                        Style="text-decoration: underline; cursor: pointer; color: Blue;" Text='<%# DataBinder.Eval(Container, "DataItem.FLDWORKPERMIT").ToString().Length > 20 ? DataBinder.Eval(Container, "DataItem.FLDWORKPERMIT").ToString().Substring(0, 20) : DataBinder.Eval(Container, "DataItem.FLDWORKPERMIT").ToString() %>'>
                                    </asp:HyperLink>
                                    <telerik:RadLabel ID="lblWorkPermit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDWORKPERMITNA").ToString().Length > 20 ? DataBinder.Eval(Container, "DataItem.FLDWORKPERMITNA").ToString().Substring(0, 20) : DataBinder.Eval(Container, "DataItem.FLDWORKPERMITNA").ToString() %>'></telerik:RadLabel>
                                    <eluc:ToolTip ID="ucToolTipWorkPermit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKPERMIT") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblWorkPermitIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKPERMITID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <eluc:WorkPermitByCompany runat="server" ID="ucWorkPermitEdit" AppendDataBoundItems="true"
                                         CategoryNumber="3" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:WorkPermitByCompany runat="server" ID="ucWorkPermitAdd" AppendDataBoundItems="true"
                                         CategoryNumber="3" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Risk Assessment">
                                <HeaderStyle Width="150px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRAId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENTID") %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkRA" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRAREFNO") %>'></asp:LinkButton>
                                    <telerik:RadLabel ID="lblRA" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRAREFNONA") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblEngineJHAList" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMPORTEDJHA") %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="imgEngineJHAList" AlternateText="Show Imported JHA"
                                        runat="server" ToolTip="Show Imported JHA" >
                                           <span class="icon"><i class="fas fa-clipboard-list"></i></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <span id="spnRAEditForm">
                                        <telerik:RadTextBox ID="txtRANumberEdit" runat="server"  Enabled="false"
                                            MaxLength="50" Width="50px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRAREFNO") %>'>
                                        </telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtRAEdit" runat="server"  Enabled="false" MaxLength="50"
                                            Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENT") %>'>
                                        </telerik:RadTextBox>
                                        <asp:LinkButton ID="imgShowRAEdit" runat="server" 
                                            ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" >
                                              <span class="icon"><i class="fas fa-tasks"></i></span>
                                        </asp:LinkButton>
                                        <telerik:RadTextBox ID="txtRAIdEdit" runat="server"  MaxLength="20" Width="0px"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENTID") %>'>
                                        </telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtRaTypeEdit" runat="server"  MaxLength="2" Width="0px"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENTTYPE") %>'>
                                        </telerik:RadTextBox>
                                    </span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <span id="spnRAAddForm">
                                        <telerik:RadTextBox ID="txtRANumberAdd" runat="server"  Enabled="false"
                                            MaxLength="50" Width="50px">
                                        </telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtRAAdd" runat="server"  Enabled="false" MaxLength="50"
                                            Width="80px">
                                        </telerik:RadTextBox>
                                        <asp:LinkButton ID="imgShowRAAdd" runat="server" 
                                            ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" >
                                              <span class="icon"><i class="fas fa-tasks"></i></span>
                                        </asp:LinkButton>
                                        <telerik:RadTextBox ID="txtRAIdAdd" runat="server"  MaxLength="20" Width="0px"></telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtRaTypeAdd" runat="server"  MaxLength="2" Width="0px"></telerik:RadTextBox>
                                    </span>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Est Start Time">
                                <HeaderStyle Width="80px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblStartTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTARTDATETIME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number MaskText="##:##" ID="txtStartTimeEdit" runat="server"  Width="100%" />

                                    hrs
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number MaskText="##:##" ID="txtStartTimeAdd" runat="server"  Width="100%" />

                                    hrs
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Est End Time">
                                <HeaderStyle Width="80px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblEndTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENDDATETIME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number MaskText="##:##" ID="txtEndTimeEdit" runat="server"  Width="100%" />

                                    hrs
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number MaskText="##:##" ID="txtEndTimeAdd" runat="server"  Width="100%" />

                                    hrs
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">
                                <ItemStyle Wrap="false" HorizontalAlign="Left" />

                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Edit"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                        ToolTip="Edit">
                                         <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                    <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Delete"
                                        CommandName="ENGINEDELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                        ToolTip="Delete">
                                        <span class="icon"><i class="fa fa-trash"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save"
                                        CommandName="UPDATE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                        ToolTip="Save">
                                        <span class="icon"><i class="fas fa-save"></i></span>
                                    </asp:LinkButton>
                                    <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:LinkButton Width="20px" Height="20px" n runat="server" AlternateText="Cancel"
                                        CommandName="CANCEL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel">
                                        <span class="icon"><i class="fas fa-times-circle"></i></span>
                                    </asp:LinkButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save"
                                        CommandName="ENGINEADD" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
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
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>

                <br />
                <telerik:RadLabel ID="lblCatering" runat="server" Text="Catering" Style="font-weight: bold"></telerik:RadLabel>

                <%-- <asp:GridView ID="gvCatering" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="gvCatering_RowDataBound" OnRowCommand="gvCatering_RowCommand"
                        OnSelectedIndexChanging="gvCatering_SelectedIndexChanging" OnRowCreated="gvCatering_RowCreated"
                        OnPreRender="gvCatering_PreRender" ShowFooter="true" Style="margin-bottom: 0px"
                        EnableViewState="false" DataKeyNames="FLDWORKPLANACTIVITYID">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvCatering" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" EnableViewState="false"
                    GroupingEnabled="false" EnableHeaderContextMenu="true"
                    OnNeedDataSource="gvCatering_NeedDataSource"
                    OnItemDataBound="gvCatering_ItemDataBound"
                    OnItemCommand="gvCatering_ItemCommand">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed" ShowFooter="true">
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


                            <telerik:GridButtonColumn Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <telerik:GridTemplateColumn>
                                <HeaderStyle Width="30px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Edit" 
                                        CommandName="CATERINGCLEAR" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdClear"
                                        ToolTip="Clear Selection">
                                        <span class="icon"><i class="fas fa-eraser"></i></span>
                                    </asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:RadioButton ID="rdbUser" runat="server" OnClick="javascript:SelectSingleRadiobutton(this.id)" />
                                    <telerik:RadLabel ID="lblWorkPlanActivityId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKPLANACTIVITYID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <telerik:RadLabel ID="lblWorkPlanActivityLineItemId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKPLANACTIVITYLINEITEMID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="S.No">
                                <HeaderStyle Width="60px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>'></telerik:RadLabel>

                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblWorkPlanActivityIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKPLANACTIVITYID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <telerik:RadLabel ID="lblWorkPlanActivityLineItemIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKPLANACTIVITYLINEITEMID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <eluc:Number ID="ucSNoEdit" runat="server" Width="100%"
                                        MaskText="###" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number ID="ucSNoAdd" runat="server" Width="100%"
                                        MaskText="###" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Activity">
                                <HeaderStyle Width="150px" />

                                <ItemStyle Wrap="true" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblActivity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITY") %>' ></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox ID="txtActivityEdit" runat="server" CssClass="input_mandatory" MaxLength="200" Width="100%"
                                        TextMode="MultiLine" Rows="2" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITY") %>'>
                                    </telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtActivityAdd" runat="server" CssClass="input_mandatory" MaxLength="200"
                                        TextMode="MultiLine" Rows="2" Width="100%">
                                    </telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Person Incharge">
                                <HeaderStyle Width="150px" />

                                <ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblIncharge" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICNAMERANK") %>'></telerik:RadLabel>
                                    <eluc:ToolTip ID="ucToolTipPICName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICNAMERANK") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <span id="spnCrewInChargeEditRiskAssessment">
                                        <telerik:RadTextBox ID="txtCrewNameEdit" runat="server"  Enabled="false"
                                            MaxLength="50" Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCHARGENAME") %>'>
                                        </telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtCrewRankEdit" runat="server"  Enabled="false"
                                            MaxLength="50" Width="50px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCHARGERANK") %>'>
                                        </telerik:RadTextBox>
                                        <asp:LinkButton ID="imgShowCrewInChargeEdit" runat="server"
                                            ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" >
                                              <span class="icon"><i class="fas fa-tasks"></i></span>
                                        </asp:LinkButton>
                                        <telerik:RadTextBox ID="txtCrewIdEdit" runat="server"  MaxLength="0" Width="0px"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCHARGE") %>'>
                                        </telerik:RadTextBox>
                                    </span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <span id="spnCrewInChargeAddRiskAssessment">
                                        <telerik:RadTextBox ID="txtCrewNameAdd" runat="server"  Enabled="false"
                                            MaxLength="50" Width="80px">
                                        </telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtCrewRankAdd" runat="server"  Enabled="false"
                                            MaxLength="50" Width="50px">
                                        </telerik:RadTextBox>
                                        <asp:LinkButton ID="imgShowCrewInChargeAdd" runat="server" 
                                            ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" >
                                              <span class="icon"><i class="fas fa-tasks"></i></span>
                                        </asp:LinkButton>
                                        <telerik:RadTextBox ID="txtCrewIdAdd" runat="server"  MaxLength="20" Width="0px"></telerik:RadTextBox>
                                    </span>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Job Hazard Analysis">
                                <HeaderStyle Width="150px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkHazardNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNUMBER") %>'></asp:LinkButton>
                                    <telerik:RadLabel ID="lblHazardNumber" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNUMBERNA") %>' ></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblHazardId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBHAZARDID") %>' Visible="false"></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <span id="spnHazardEditRiskAssessment">
                                        <telerik:RadTextBox ID="txtHazardNumberEdit" runat="server"  Enabled="false"
                                            MaxLength="50" Width="50px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNUMBER") %>'>
                                        </telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtHazardEdit" runat="server"  Enabled="false" MaxLength="50"
                                            Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOB") %>'>
                                        </telerik:RadTextBox>
                                        <asp:LinkButton ID="imgShowHazardEdit" runat="server" 
                                            ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" >
                                              <span class="icon"><i class="fas fa-tasks"></i></span>
                                        </asp:LinkButton>
                                        <telerik:RadTextBox ID="txtHazardIdEdit" runat="server"  Width="0px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBHAZARDID") %>'></telerik:RadTextBox>
                                    </span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <span id="spnHazardAddRiskAssessment">
                                        <telerik:RadTextBox ID="txtHazardNumberAdd" runat="server"  Enabled="false"
                                            MaxLength="50" Width="50px">
                                        </telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtHazardAdd" runat="server"  Enabled="false" MaxLength="50"
                                            Width="80px">
                                        </telerik:RadTextBox>
                                        <asp:LinkButton ID="imgShowHazardAdd" runat="server" 
                                            ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" >
                                              <span class="icon"><i class="fas fa-tasks"></i></span>
                                        </asp:LinkButton>
                                        <telerik:RadTextBox ID="txtHazardIdAdd" runat="server"  Width="0px"></telerik:RadTextBox>
                                    </span>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Work Permit">
                                <HeaderStyle Width="150px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkWorkPermit" runat="server" Target="_blank"
                                        Style="text-decoration: underline; cursor: pointer; color: Blue;" Text='<%# DataBinder.Eval(Container, "DataItem.FLDWORKPERMIT").ToString().Length > 20 ? DataBinder.Eval(Container, "DataItem.FLDWORKPERMIT").ToString().Substring(0, 20) : DataBinder.Eval(Container, "DataItem.FLDWORKPERMIT").ToString() %>'>
                                    </asp:HyperLink>
                                    <telerik:RadLabel ID="lblWorkPermit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDWORKPERMITNA").ToString().Length > 20 ? DataBinder.Eval(Container, "DataItem.FLDWORKPERMITNA").ToString().Substring(0, 20) : DataBinder.Eval(Container, "DataItem.FLDWORKPERMITNA").ToString() %>'></telerik:RadLabel>
                                    <eluc:ToolTip ID="ucToolTipWorkPermit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKPERMIT") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblWorkPermitIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKPERMITID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <eluc:WorkPermitByCompany runat="server" ID="ucWorkPermitEdit" AppendDataBoundItems="true"
                                         CategoryNumber="3" Width="100%"/>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:WorkPermitByCompany runat="server" ID="ucWorkPermitAdd" AppendDataBoundItems="true"
                                         CategoryNumber="3" Width="100%"/>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Risk Assessment">
                                <HeaderStyle Width="150px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRAId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENTID") %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkRA" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRAREFNO") %>' Width="175px"></asp:LinkButton>
                                    <telerik:RadLabel ID="lblRA" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRAREFNONA") %>' Width="175px"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblCateringJHAList" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMPORTEDJHA") %>'></telerik:RadLabel>
                                    <asp:LinkButton  ID="imgCateringJHAList" AlternateText="Show Imported JHA"
                                        runat="server" ToolTip="Show Imported JHA" >
                                           <span class="icon"><i class="fas fa-clipboard-list"></i></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <span id="spnRAEditRiskAssessment">
                                        <telerik:RadTextBox ID="txtRANumberEdit" runat="server"  Enabled="false"
                                            MaxLength="50" Width="50px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRAREFNO") %>'>
                                        </telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtRAEdit" runat="server"  Enabled="false" MaxLength="50"
                                            Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENT") %>'>
                                        </telerik:RadTextBox>
                                        <asp:LinkButton ID="imgShowRAEdit" runat="server" 
                                            ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" >
                                              <span class="icon"><i class="fas fa-tasks"></i></span>
                                        </asp:LinkButton>
                                        <telerik:RadTextBox ID="txtRAIdEdit" runat="server"  MaxLength="20" Width="0px"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENTID") %>'>
                                        </telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtRaTypeEdit" runat="server"  MaxLength="2" Width="0px"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENTTYPE") %>'>
                                        </telerik:RadTextBox>
                                    </span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <span id="spnRAAddRiskAssessment">
                                        <telerik:RadTextBox ID="txtRANumberAdd" runat="server"  Enabled="false"
                                            MaxLength="50" Width="50px">
                                        </telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtRAAdd" runat="server"  Enabled="false" MaxLength="50"
                                            Width="80px">
                                        </telerik:RadTextBox>
                                        <asp:LinkButton ID="imgShowRAAdd" runat="server" 
                                            ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" >
                                              <span class="icon"><i class="fas fa-tasks"></i></span>
                                        </asp:LinkButton>
                                        <telerik:RadTextBox ID="txtRAIdAdd" runat="server"  MaxLength="20" Width="0px"></telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtRaTypeAdd" runat="server"  MaxLength="2" Width="0px"></telerik:RadTextBox>
                                    </span>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Est Start Time">
                                <HeaderStyle Width="80px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblStartTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTARTDATETIME") %>' ></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number MaskText="##:##" ID="txtStartTimeEdit" runat="server"  Width="100%" />

                                    hrs
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number MaskText="##:##" ID="txtStartTimeAdd" runat="server"  Width="100%" />

                                    hrs
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Est End Time">
                                <HeaderStyle Width="80px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblEndTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENDDATETIME") %>' ></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number MaskText="##:##" ID="txtEndTimeEdit" runat="server"  Width="100%" />

                                    hrs
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number MaskText="##:##" ID="txtEndTimeAdd" runat="server"  Width="100%" />

                                    hrs
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">
                                <HeaderStyle Width="50px" />
                                <ItemStyle Wrap="false" HorizontalAlign="Left" />

                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="40px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Edit"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                        ToolTip="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                    <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Delete"
                                        CommandName="CATERINGDELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                        ToolTip="Delete">
                                            <span class="icon"><i class="fa fa-trash"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save"
                                        CommandName="UPDATE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                        ToolTip="Save">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                                    </asp:LinkButton>
                                    <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Cancel"
                                        CommandName="CANCEL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel">
                                            <span class="icon"><i class="fas fa-times-circle"></i></span>
                                    </asp:LinkButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save"
                                        CommandName="CATERINGADD" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
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
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>

           
            <br />
            <div id="divRemarks" style="position: relative; z-index: 2">
                <table id="tblRemarks" width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblRemarks" runat="server" Text=" Remarks"></telerik:RadLabel>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="4" Width="800px" ></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
