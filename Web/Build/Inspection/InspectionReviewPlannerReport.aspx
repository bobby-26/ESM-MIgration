<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionReviewPlannerReport.aspx.cs" Inherits="InspectionReviewPlannerReport" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselName" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmReviewPlanner" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlReviewPlanner">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="frmTitle" Text="Audit/Inspection Planner"></eluc:Title>
                    <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuReviewPlannerMain" runat="server" 
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <table id="tblBudgetGroupAllocationSearch" width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblVessel" runat="server" Text="Vessel "></asp:Literal>
                        </td>
                       <td>
                                <eluc:VesselName ID="ucVesselName" AppendDataBoundItems="true" runat="server" CssClass="input" />
                         </td>
                        <td>
                            <asp:Literal ID="lblFleet" runat="server" Text="Fleet"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Fleet ID="ucFleet" runat="server" CssClass="input" AppendDataBoundItems="true"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lblPrincipal" runat="server" Text="Principal"></asp:Literal>
                        </td>
                        <td>
                            <eluc:AddressType runat="server" ID="ucPrincipal" AddressType="128" CssClass="input" AppendDataBoundItems="true" Width="80%" />
                        </td>
                        <td>
                           <asp:Literal ID="lblCharterer" runat="server" Text="Charterer"></asp:Literal>
                        </td>
                        <td>
                            <eluc:AddressType runat="server" ID="ucCharterer" CssClass="input" AppendDataBoundItems="true" Width="80%"  />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblDueDateFrom" runat="server" Text="Due Date From"></asp:Literal>
                        </td>
                        <td>
                            <eluc:UserControlDate ID="ucDueDateFrom" runat="server" CssClass="input" DatePicker="true"/>
                        </td>
                        <td>
                            <asp:Literal ID="lblDueDateTo" runat="server" Text="Due Date To"></asp:Literal>
                        </td>
                        <td>
                            <eluc:UserControlDate ID="ucDueDateTo" runat="server" CssClass="input" DatePicker="true"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Hard ID="ucStatus" runat="server" AppendDataBoundItems="true" HardTypeCode="146" CssClass="input"
                             ShortNameFilter="PLA,ASG" />
                        </td>
                    </tr>
                    
                </table>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuReviewPlanner" runat="server" OnTabStripCommand="MenuReviewPlanner_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvReviewPlanner" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" AllowSorting="true" OnSorting="gvReviewPlanner_Sorting"
                        EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" BorderColor="#FF0066" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="Reference Number">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblReferenceNumberHeader" runat="server" CommandName="Sort" CommandArgument="FLDREFERENCENUMBER"
                                        ForeColor="White">Reference Number&nbsp;</asp:LinkButton>
                                    <img id="FLDREFERENCENUMBER" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblReferenceNumber" runat="server" Visible="TRUE" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENUMBER") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDREVIEWNAME"
                                        ForeColor="White">Name&nbsp;</asp:LinkButton>
                                    <img id="FLDREVIEWNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblReviewName" runat="server" Visible="TRUE" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVIEWNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vessel">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblVesselHeader" runat="server" CommandName="Sort" CommandArgument="FLDVESSELSHORTCODE"
                                        ForeColor="White">Vessel&nbsp;</asp:LinkButton>
                                    <img id="FLDVESSELSHORTCODE" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELSHORTCODE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Last Done Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblLastDoneDateHeader" runat="server" >Last Done Date</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblLastDoneDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDLASTDONEDATE")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Due Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lbLDueDateHeader" runat="server" >Due Date</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDueDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDREVIEWSTARTDATE")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Planned Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblPlannedDateHeader" runat="server" >Planned Date</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPlannedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDRANGEFROMDATE")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name Of Inspector">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblInspectorNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAMEOFINSPECTOR"
                                        ForeColor="White">Name of Inspector&nbsp;</asp:LinkButton>
                                    <img id="FLDNAMEOFINSPECTOR" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblInspectorName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFINSPECTOR") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Port of Audit">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblAuditPortHeader" runat="server" CommandName="Sort" CommandArgument="FLDSEAPORTNAME"
                                        ForeColor="White">Port of Audit&nbsp;</asp:LinkButton>
                                    <img id="FLDSEAPORTNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAuditPort" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblStatusHeader" runat="server" CommandName="Sort" CommandArgument="FLDSTATUSNAME"
                                        ForeColor="White">Status&nbsp;</asp:LinkButton>
                                    <img id="FLDSTATUSNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblstatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage" style="position: relative;">
                    <table width="100%" border="0" cellpadding="1" cellspacing="1" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap align="center">
                                <asp:Label ID="lblPagenumber" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPages" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecords" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap align="left" width="50px">
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap align="right" width="50px">
                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap align="center">
                                <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
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
