<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionScheduleByCompany.aspx.cs"
    Inherits="InspectionScheduleByCompany" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="DepartmentType" Src="~/UserControls/UserControlDepartmentType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Inspection Schedule By Company</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmInspectionScheduleByCompany" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlBudgetGroup">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
                    <div class="subHeader" style="position: relative; right: 0px">
                        <eluc:Title runat="server" ID="ucTitle" Text="CDI / SIRE Schedule" />
                        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />
                        <eluc:Status runat="server" ID="ucStatus" />
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                    <eluc:TabStrip ID="MenuGeneral" runat="server" OnTabStripCommand="MenuGeneral_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <table id="tblBudgetGroupAllocationSearch" width="70%">
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkShowAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkShowAll_Changed"
                                Visible="false" />
                        </td>
                    </tr>
                </table>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuScheduleGroup" runat="server" OnTabStripCommand="BudgetGroup_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <%--<div id="divGrid" style="position: relative; z-index: +1">--%>
                <div div id="divGrid" style="width: 100%; overflow-x: auto;">
                    <asp:GridView ID="gvScheduleForCompany" runat="server" AutoGenerateColumns="False"
                        Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvScheduleForCompany_RowCommand"
                        OnRowDataBound="gvScheduleForCompany_ItemDataBound" OnRowCancelingEdit="gvScheduleForCompany_RowCancelingEdit"
                        OnRowDeleting="gvScheduleForCompany_RowDeleting" OnRowEditing="gvScheduleForCompany_RowEditing"
                        ShowFooter="false" ShowHeader="true" EnableViewState="false" OnSorting="gvScheduleForCompany_Sorting"
                        AllowSorting="true" OnSelectedIndexChanging="gvScheduleForCompany_SelectedIndexChanging"
                        OnRowUpdating="gvScheduleForCompany_RowUpdating" GridLines="None">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Image ID="imgFlag" runat="server" Visible="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkVesselHeader" runat="server" CommandName="Sort" CommandArgument="FLDVESSELNAME">Vessel &nbsp;</asp:LinkButton>
                                    <img id="FLDVESSELNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkVessel" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'
                                        CommandName="SELECT" CommandArgument='<%# Container.DataItemIndex%>'></asp:LinkButton>
                                    <asp:Label ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                    <asp:Label ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                        Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <HeaderTemplate>
                                    M/C
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblManual" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISMANUAL") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkInspectionNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDCOMPANYNAME">Company&nbsp;</asp:LinkButton>
                                    <img id="FLDCOMPANYNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCompanyName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYNAME") %>'></asp:Label>
                                    <asp:Label ID="lblCompanyId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYID") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblScheduleId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHEDULEID") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblScheduleByCompanyId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHEDULEBYCOMPANYID") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblInspectionId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONID") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblIsManual" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISMANUALINSPECTION") %>'
                                        Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTypeHeader" runat="server">Type</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVesselType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblInspectionHeader" runat="server">Inspection</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblInspection" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONSHORTCODE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblLastDoneHeader" runat="server">Last Done</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblLastDoneDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDLASTDONEDATE")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblInspectionIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONID") %>'
                                        Visible="false"></asp:Label>
                                    <eluc:Date ID="ucLastDoneDateEdit" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDLASTDONEDATE")) %>'
                                        DatePicker="true" CssClass="input" OnTextChangedEvent="ucLastDoneDateEdit_TextChanged"
                                        AutoPostBack="true" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkDueHeader" runat="server" CommandName="Sort" CommandArgument="FLDDUEDATE">Due</asp:LinkButton>
                                    <img id="FLDDUEDATE" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDueDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDUEDATE")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="ucDueDateEdit" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDUEDATE")) %>'
                                        DatePicker="true" CssClass="input" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblBasisHeader" runat="server"> Basis</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkBasisDetails" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASISDETAILS") %>'
                                        CommandName="SHOW" CommandArgument='<%# Container.DataItemIndex%>'></asp:LinkButton>
                                    <asp:Label ID="lblBasisDetails" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASISDETAILS") %>'></asp:Label>
                                    <asp:Label ID="lblBasisId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASISID") %>'
                                        Visible="false"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <span id="spanBasisInspectionSchedule">
                                        <asp:TextBox ID="txtCompany" runat="server" CssClass="input" Enabled="false" Width="40px"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASISCOMPANYNAME") %>'></asp:TextBox>
                                        <asp:TextBox ID="txtBasis" runat="server" CssClass="input" Enabled="false" Width="80px"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASISSCHEDULENUMBER") %>'></asp:TextBox>
                                        <img id="imgBasis" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>" style="cursor: pointer;
                                            vertical-align: middle; padding-bottom: 3px;" />
                                        <asp:TextBox ID="txtBasisScheduleId" runat="server" CssClass="hidden" Width="0px"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASISID") %>'></asp:TextBox>
                                        <asp:ImageButton runat="server" ID="imgClearBasis" ToolTip="Clear Basis" CommandName="CLEAR"
                                            CommandArgument='<%# Container.DataItemIndex%>' OnClick="ClearBasis" ImageUrl="<%$ PhoenixTheme:images/clear-filter.png %>" />
                                    </span>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblLastDoneRefHeader" runat="server"> Last Done Ref</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblScheduleNumber" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREVIOUSSCHEDULENUMBER") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkScheduleNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREVIOUSSCHEDULENUMBER") %>'
                                        CommandName="SHOW" CommandArgument='<%# Container.DataItemIndex%>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkPlannedHeader" runat="server" CommandName="Sort" CommandArgument="FLDPLANNEDDATE">Planned</asp:LinkButton>
                                    <img id="FLDPLANNEDDATE" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPlannedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPLANNEDDATE")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblPlannedDateEdit" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPLANNEDDATE")) %>'></asp:Label>
                                    <eluc:Date ID="ucPlannedDateEdit" runat="server" Visible="false" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPLANNEDDATE")) %>'
                                        DatePicker="true" CssClass="input" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblPlannedPortHeader" runat="server"> Planned Port</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPlannedPort" runat="server" Width="90px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDSEAPORTNAME").ToString().Substring(0, 10)+ "..." : DataBinder.Eval(Container, "DataItem.FLDSEAPORTNAME").ToString() %>'></asp:Label>
                                    <eluc:ToolTip ID="ucToolTipPort" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblPlannedPortEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'></asp:Label>
                                    <eluc:SeaPort ID="ucSeaportEdit" runat="server" Visible="false" Width="90px" AppendDataBoundItems="true"
                                        CssClass="input" SelectedSeaport='<%# DataBinder.Eval(Container,"DataItem.FLDPORTID") %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblInspectorHeader" runat="server">Inspector</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblInspector" runat="server" Width="90px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFINSPECTOR").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDNAMEOFINSPECTOR").ToString().Substring(0, 10)+ "..." : DataBinder.Eval(Container, "DataItem.FLDNAMEOFINSPECTOR").ToString() %>'></asp:Label>
                                    <eluc:ToolTip ID="ucToolTipInspector" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFINSPECTOR") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblInspectorEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFINSPECTOR") %>'></asp:Label>
                                    <asp:TextBox ID="txtInspectorEdit" runat="server" Width="90px" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFINSPECTOR") %>'
                                        CssClass="input"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblStatusHeader" runat="server"> Status</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblScheduleStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCHEDULESTATUS") %>'></asp:Label>
                                    <asp:Label ID="lblStatusId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">Action</asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="60px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <%--<asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>--%>
                                    <asp:ImageButton runat="server" AlternateText="Plan" ImageUrl="<%$ PhoenixTheme:images/scheduled-tasks-icon.png %>"
                                        CommandName="CREATESCHEDULE" CommandArgument="<%# Container.DataItemIndex %>"
                                        ID="imgCreateSchedule" ToolTip="Plan"></asp:ImageButton>
                                    <img id="Img4" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Report" ImageUrl="<%$ PhoenixTheme:images/Modify.png %>"
                                        CommandName="REPORT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdReport"
                                        ToolTip="Report"></asp:ImageButton>
                                    <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="NEWSCHEDULE" ImageUrl="<%$ PhoenixTheme:images/new.png %>"
                                        CommandName="NEWSCHEDULE" CommandArgument="<%# Container.DataItemIndex %>" Visible="false"
                                        ID="imgNewSchedule" ToolTip="New Schedule"></asp:ImageButton>
                                    <img id="Img6" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="UNPLAN" ImageUrl="<%$ PhoenixTheme:images/unplan-inspection.png %>"
                                        CommandName="UNPLAN" CommandArgument="<%# Container.DataItemIndex %>" Visible="false"
                                        ID="imgUnPlan" ToolTip="Un Plan"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap="nowrap" align="center">
                                <asp:Label ID="lblPagenumber" runat="server"> </asp:Label>
                                <asp:Label ID="lblPages" runat="server"> </asp:Label>
                                <asp:Label ID="lblRecords" runat="server"> </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap="nowrap" align="left" width="50px">
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap="nowrap" align="right" width="50px">
                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap="nowrap" align="center">
                                <eluc:Number ID="txtnopage" Width="30px" runat="server" CssClass="input" MaxLength="9" />
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
                <table cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <img id="Img1" src="<%$ PhoenixTheme:images/red-symbol.png%>" runat="server" />
                        </td>
                        <td>
                            <asp:Literal ID="lblOverDue" runat="server" Text=" * Overdue"></asp:Literal>
                        </td>
                        <td>
                            <img id="Img2" src="<%$ PhoenixTheme:images/yellow-symbol.png%>" runat="server" />
                        </td>
                        <td>
                            <asp:Literal ID="lblDueWithin30days" runat="server" Text=" * Due within 30 days"></asp:Literal>
                        </td>
                        <td>
                            <img id="Img5" src="<%$ PhoenixTheme:images/green-symbol.png%>" runat="server" />
                        </td>
                        <td>
                            <asp:Literal ID="lblDueWithin60Days" runat="server" Text=" * Due within 60 days"></asp:Literal>
                        </td>
                    </tr>
                </table>
            </div>
            <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="btnConfirm_Click" OKText="Yes"
                CancelText="No" />
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
