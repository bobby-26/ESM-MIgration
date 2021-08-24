<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMainFleetRACargoList.aspx.cs" Inherits="InspectionMainFleetRACargoList" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Risk Assessment Cargo</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="div1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCargo">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Status runat="server" ID="ucStatus" />
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
                    <div class="subHeader" style="position: relative; right: 0px">
                        <eluc:Title runat="server" ID="ucTitle" Text="Cargo" ShowMenu="true" />
                        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuCargoMain" runat="server" TabStrip="false" OnTabStripCommand="MenuCargoMain_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuCargo" runat="server" OnTabStripCommand="MenuCargo_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <asp:GridView ID="gvRiskAssessmentCargo" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                    Font-Size="11px" Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false"
                    OnRowDataBound="gvRiskAssessmentCargo_RowDataBound" DataKeyNames="FLDRISKASSESSMENTCARGOID"
                    OnRowCommand="gvRiskAssessmentCargo_RowCommand" OnSorting="gvRiskAssessmentCargo_Sorting"  GridLines="None">
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="5px" />
                    <Columns>
                        <asp:TemplateField>
                           <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                           <ItemTemplate>
                               <asp:Image ID="imgFlag" runat="server" Visible="false" />
                           </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ref Number">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblRefNoHeader" runat="server">Ref Number</asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblRefNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER")  %>'></asp:Label>
                                <asp:Label ID="lblReferencid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCEID")  %>'></asp:Label>
                                <asp:Label ID="lblInstallcode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSTALLCODE")  %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblVesselHeader" runat="server">Vessel</asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblVesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID")  %>'></asp:Label>
                                <asp:Label ID="lblIsCreatedByOffice" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISCREATEDBYOFFICE")  %>'></asp:Label>
                                <%# ((DataRowView)Container.DataItem)["FLDVESSELNAME"] %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblDateHeader" runat="server">Prepared</asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDDATE"]) %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkintendedWorkDateHeader" runat="server" CommandName="Sort" CommandArgument="FLDINTENDEDWORKDATE">Intended Work</asp:LinkButton>
                                <img id="FLDINTENDEDWORKDATE" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDINTENDEDWORKDATE"])%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblTypeHeader" runat="server">Type</asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE")  %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Risks/Aspects">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblActivityConditionsHeader" runat="server">Activity / Conditions</asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblRiskAssessmentCargoID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENTCARGOID")  %>'
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblRevisionno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONNO")  %>'
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblWorkActivity" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDACTIVITYCONDITIONS").ToString().Length > 35 ? DataBinder.Eval(Container, "DataItem.FLDACTIVITYCONDITIONS").ToString().Substring(0, 35) + "..." : DataBinder.Eval(Container, "DataItem.FLDACTIVITYCONDITIONS").ToString() %>'></asp:Label>
                                <eluc:ToolTip ID="ucWorkActivity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITYCONDITIONS") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblRevisionNoHeader" runat="server">Revision No</asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblRevno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONNO")  %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblStatusHeader" runat="server">Status</asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS")  %>'></asp:Label>
                                <asp:Label ID="lblStatusName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME")  %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblApprovedByHeader" runat="server">Approved by</asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblApprovedBy" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDAPPROVEDBY").ToString().Length > 25 ? DataBinder.Eval(Container, "DataItem.FLDAPPROVEDBY").ToString().Substring(0, 25) + "..." : DataBinder.Eval(Container, "DataItem.FLDAPPROVEDBY").ToString() %>'></asp:Label>
                                <eluc:ToolTip ID="ucApprovedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVEDBY") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblActionHeader" runat="server">Action</asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDITROW" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Approve" ImageUrl="<%$ PhoenixTheme:images/approve.png %>"
                                    CommandName="APPROVE" CommandArgument="<%# Container.DataItemIndex %>" ID="imgApprove"
                                    ToolTip="Approve"></asp:ImageButton>
                                <img id="Img5" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Issue" ImageUrl="<%$ PhoenixTheme:images/approved.png %>"
                                    CommandName="ISSUE" CommandArgument="<%# Container.DataItemIndex %>" ID="imgIssue"
                                    ToolTip="Issue"></asp:ImageButton>
                                <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Revision" ImageUrl="<%$ PhoenixTheme:images/risk-assessment.png %>"
                                    CommandName="REVISION" CommandArgument="<%# Container.DataItemIndex %>" ID="imgrevision"
                                    ToolTip="Create Revision"></asp:ImageButton>
                                <img id="Img6" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cargo PDF" CommandName="RACARGO"
                                    CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="<%$ PhoenixTheme:images/BarChart.png %>"
                                    ID="cmdRACargo" ToolTip="Show PDF"></asp:ImageButton>
                                <img id="Img4" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="View Revisions" ImageUrl="<%$ PhoenixTheme:images/requisition.png %>"
                                    CommandName="VIEWREVISION" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdRevision"
                                    ToolTip="View Revisions"></asp:ImageButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Copy template" ImageUrl="<%$ PhoenixTheme:images/copy.png %>"
                                    CommandName="COPYTEMPLATE" CommandArgument="<%# Container.DataItemIndex %>" ID="imgCopyTemplate"
                                    ToolTip="Copy template"></asp:ImageButton>
                                <img id="Img7" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Propose template" ImageUrl="<%$ PhoenixTheme:images/add_task.png %>"
                                    CommandName="PROPOSETEMPLATE" CommandArgument="<%# Container.DataItemIndex %>" ID="imgProposeTemplate"
                                    ToolTip="Propose Standard Template"></asp:ImageButton>
                                <%--<img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Copy" ImageUrl="<%$ PhoenixTheme:images/vessel.png %>"
                                    CommandName="COPYTOVESSEL" CommandArgument="<%# Container.DataItemIndex %>" ID="imgCopy"
                                    ToolTip="Copy To Vessel"></asp:ImageButton>--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <table width="100%" border="0" class="datagrid_pagestyle">
                    <tr>
                        <td nowrap="nowrap" align="center">
                            <asp:Label ID="lblPagenumber" runat="server">
                            </asp:Label>
                            <asp:Label ID="lblPages" runat="server">
                            </asp:Label>
                            <asp:Label ID="lblRecords" runat="server">
                            </asp:Label>&nbsp;&nbsp;
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
                            <eluc:Number ID="txtnopage" runat="server"  Width="30px" MaxLength ="9" CssClass="input">
                            </eluc:Number>
                            <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                Width="40px"></asp:Button>
                        </td>
                    </tr>
                </table>
                 <table cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <img id="Img11" src="<%$ PhoenixTheme:images/red-symbol.png%>" runat="server" />
                        </td>
                        <td>
                            <asp:Literal ID="lblOverDue" runat="server" Text=" * Overdue"></asp:Literal>
                        </td>
                        <td>
                            <img id="Img12" src="<%$ PhoenixTheme:images/yellow-symbol.png%>" runat="server" />
                        </td>
                        <td>
                            <asp:Literal ID="lblDueWithin2weeks" runat="server" Text=" * Due within 2 Weeks"></asp:Literal>
                        </td>
                        <td>
                            <img id="Img8" src="<%$ PhoenixTheme:images/green.png%>" runat="server" />
                        </td>
                        <td>
                            <asp:Literal ID="lblcopiedST" runat="server" Text=" * Copied from Standard Template"></asp:Literal>
                        </td>
                    </tr>
                </table>
            </div>
            <eluc:Confirm ID="ucConfirmApprove" runat="server" OnConfirmMesage="btnConfirmApprove_Click"
                OKText="Yes" CancelText="No" />
            <eluc:Confirm ID="ucConfirmIssue" runat="server" OnConfirmMesage="btnConfirmIssue_Click"
                OKText="Yes" CancelText="No" />
            <eluc:Confirm ID="ucConfirmRevision" runat="server" OnConfirmMesage="btnConfirmRevision_Click"
                OKText="Yes" CancelText="No" />
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>

