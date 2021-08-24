<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionScheduleByCompanyHistory.aspx.cs" Inherits="InspectionScheduleByCompanyHistory" %>

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
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Inspection Schedule By Company History</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Schedule History" />
                        <eluc:Status runat="server" ID="ucStatus" />
                    </div>
                    <div class="navBudget" style="top: 0px; right: 0px; position: absolute;">
                        <eluc:TabStrip ID="MenuSchedule" runat="server" OnTabStripCommand="MenuSchedule_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                    </div>
                </div>
                <table id="tblBudgetGroupAllocationSearch" width="70%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                                CssClass="input" VesselsOnly="true" OnTextChangedEvent="Text_Changed" />
                        </td>
                        <td>
                            <asp:Literal ID="lblCompany" runat="server" Text="Company"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCompany" runat="server" AutoPostBack="true" CssClass="input"
                                Width="100px" OnTextChangedEvent="Text_Changed">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkShowAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkShowAll_Changed"
                                Checked="true" Visible="false" />
                        </td>
                    </tr>
                </table>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuScheduleGroup" runat="server" OnTabStripCommand="BudgetGroup_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: +1">
                    <asp:GridView ID="gvScheduleForCompany" runat="server" AutoGenerateColumns="False"
                        Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvScheduleForCompany_RowCommand"
                        OnRowDataBound="gvScheduleForCompany_ItemDataBound" 
                        ShowFooter="true" ShowHeader="true" EnableViewState="false" OnSorting="gvScheduleForCompany_Sorting"
                        AllowSorting="true" OnSelectedIndexChanging="gvScheduleForCompany_SelectedIndexChanging">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblCompanyHeader" runat="server">Company</asp:Label>
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
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblVesselHeader" runat="server">Vessel</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'
                                        CommandName="SELECT" CommandArgument='<%# Container.DataItemIndex%>'></asp:LinkButton>
                                    <asp:Label ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                        Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
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
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblLastDoneDateHeader" runat="server">Last Done Date</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblLastDoneDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDLASTDONEDATE")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="ucLastDoneDateEdit" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDLASTDONEDATE"))%>'
                                        DatePicker="true" CssClass="input" OnTextChangedEvent="ucLastDoneDateEdit_TextChanged"
                                        AutoPostBack="true" />
                                </EditItemTemplate>
                            </asp:TemplateField>                           
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:label ID="lblDueDateHeader" runat="server">Due Date</asp:label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDueDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDUEDATE"))  %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="ucDueDateEdit" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDUEDATE")) %>'
                                        DatePicker="true" CssClass="input" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblBasisHeader" runat="server">Basis</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                     <asp:Label ID="lblBasisDetails" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASISDETAILS") %>'></asp:Label>
                                    <asp:Label ID="lblBasisId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASISID") %>'
                                        Visible="false"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlBasis" runat="server" CssClass="input">
                                    </asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>                            
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                   <asp:Label ID="lblScheduleNoHeader" runat="server"> Schedule Number</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblScheduleNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCHEDULENUMBER") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblStatusHeader" runat="server">Status</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblScheduleStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCHEDULESTATUS") %>'></asp:Label>
                                </ItemTemplate>
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
                                <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input"> </asp:TextBox>
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
