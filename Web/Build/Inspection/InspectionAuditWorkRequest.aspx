<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionAuditWorkRequest.aspx.cs" Inherits="InspectionAuditWorkRequest" %>

<%@ Import Namespace="System.Data" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Discipline" Src="~/UserControls/UserControlDiscipline.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Work Requisition Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmWorkOrderRequisition" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlWorkOrderRequisition">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader">
                    <div class="divFloatLeft">
                        <eluc:Title runat="server" ID="Title1" Text="Work  Request" ShowMenu="false"></eluc:Title>
                    </div>
                    <div class="divFloat">
                        <eluc:TabStrip ID="MenuWorkOrderRequestion" runat="server" OnTabStripCommand="MenuWorkOrderRequestion_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                </div>
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblNo" runat="server" Text="Number"></asp:Literal>
                        </td>
                        <td>
                            <asp:Label Visible="false" runat="server" ID="lblWorkOrderRequisitionID"></asp:Label>
                            <asp:Label Visible="false" runat="server" ID="lblWorkOrderID"></asp:Label>
                            <asp:TextBox ID="txtWorkOrderNumber" runat="server" CssClass="input" Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                           <asp:Literal ID="lblTitle" runat="server" Text="Title"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTitle" runat="server" CssClass="input_mandatory" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblComponent" runat="server" Text="Component"></asp:Literal>
                        </td>
                        <td>
                            <span id="spnPickListComponent">
                                <asp:TextBox ID="txtComponentCode" runat="server" CssClass="input_mandatory" MaxLength="20"
                                    Enabled="false" Width="60px"></asp:TextBox>
                                <asp:TextBox ID="txtComponentName" runat="server" CssClass="input_mandatory" MaxLength="20"
                                    Enabled="false" Width="210px"></asp:TextBox>
                                <img id="imgComponent" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>"
                                    style="cursor: pointer; vertical-align: top" />
                                <asp:TextBox ID="txtComponentId" runat="server" CssClass="input" Width="10px"></asp:TextBox>
                            </span>
                        </td>
                        <td>
                            <asp:Literal ID="lblCreated" runat="server" Text="Created"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCreatedDate" runat="server" CssClass="input" Width="90px" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblJobDescribtion" runat="server" Text="Job Description"></asp:Literal>
                        </td>
                        <td>
                            <span id="spnPickListJob">
                                <asp:TextBox ID="txtJobCode" runat="server" CssClass="input readonlytextbox" MaxLength="20"
                                    ReadOnly="false" Width="60px"></asp:TextBox>
                                <asp:TextBox ID="txtJobName" runat="server" CssClass="input readonlytextbox" MaxLength="20"
                                    ReadOnly="false" Width="210px"></asp:TextBox>
                                <img id="imgJob" runat="server" onclick="return showPickList('spnPickListJob', 'codehelp1', '', '../Common/CommonPickListJob.aspx', true); "
                                    src="<%$ PhoenixTheme:images/picklist.png %>" style="cursor: pointer; vertical-align: top" />
                                <asp:TextBox ID="txtJobId" runat="server" CssClass="input" Width="10px"></asp:TextBox>
                            </span>
                        </td>
                        <td>
                            <asp:Literal ID="lblResponsibility" runat="server" Text="Responsibility"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Discipline ID="ucDiscipline" runat="server" CssClass="input" AppendDataBoundItems="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblEstimatedDuration" runat="server" Text="Estimated Duration(Hrs)"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Decimal runat="server" ID="txtDuration" Mask="99999999" CssClass="input" Width="60px" />
                        </td>
                        <td>
                            <asp:literal ID="lblUnplannedWork" runat="server" Text="Unplanned Work"></asp:literal>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkUnexpected" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lblPlannedStart" runat="server" Text="Planned Start"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPlannedStartDate" runat="server" CssClass="input_mandatory" MaxLength="20"
                                Width="90px"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="cetxtStartedDate" runat="server" Format="dd/MMM/yyyy"
                                Enabled="True" TargetControlID="txtPlannedStartDate" PopupPosition="TopLeft">
                            </ajaxToolkit:CalendarExtender>
                        </td>
                        <td>
                            <asp:Literal ID="lblPriority" runat="server" Text="Priority"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Decimal runat="server" ID="txtPriority" Mask="9" CssClass="input" Width="60px" Text="1"/>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                           <asp:Literal ID="lblWorkDetails" runat="server" Text="Work Details"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtJobDescription" runat="server" Width="300px" TextMode="MultiLine"
                                Height="50px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblDefectList" runat="server" Text="Defect List"></asp:Literal>
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chkIsDefect"  Checked="true"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblPTWApproval" runat="server" Text="PTW Approval"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Hard ID="ucWTOApproval" runat="server" CssClass="input" HardTypeCode="117"
                                AppendDataBoundItems="true" DataBoundItemName="None" />
                        </td>
                        <td>
                            <asp:Literal ID="lblMaintenanceType" runat="server" Text="Maintenance Type"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Quick ID="ucMainType" runat="server" CssClass="input" AppendDataBoundItems="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="navSelect" style="position: relative; width: 15px">
                                <eluc:TabStrip ID="MenuInspectionWorkRequest" runat="server" OnTabStripCommand="MenuInspectionWorkRequest_TabStripCommand">
                                </eluc:TabStrip>
                            </div>
                        </td>
                    </tr>
                </table>                
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvWorkOrder" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" AllowSorting="true"
                        OnSelectedIndexChanging="gvWorkOrder_SelectedIndexChanging" DataKeyNames="FLDWORKORDERID">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Image ID="imgFlag" runat="server" Visible="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblWorkorderNumberHeader" runat="server" CommandName="Sort" CommandArgument="FLDWORKORDERNUMBER"
                                        ForeColor="White">Work Order Number</asp:LinkButton>
                                    <img id="FLDWORKORDERNUMBER" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblWorkorderNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNUMBER") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblWorkorderNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDWORKORDERNAME"
                                        ForeColor="White">Work Order Title</asp:LinkButton>
                                    <img id="FLDWORKORDERNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblComponentId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDCOMPONENTID"]%>'></asp:Label>
                                    <asp:Label ID="lblWorkOrderId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDWORKORDERID"] %>'></asp:Label>
                                    <asp:LinkButton ID="lnkWorkorderName" runat="server" CommandName="Select" CommandArgument='<%# ((DataRowView)Container.DataItem)["FLDWORKORDERID"]%>'
                                        Text=' <%#((DataRowView)Container.DataItem)["FLDWORKORDERNAME"]%>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblComponentNoHeader" runat="server" >Component Number</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#((DataRowView)Container.DataItem)["FLDCOMPONENTNUMBER"]%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                   <asp:Label ID="lblComponentNameHeader" runat="server"> Component Name</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#((DataRowView)Container.DataItem)["FLDCOMPONENTNAME"]%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblPriorityHeader" runat="server" >Priority</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#((DataRowView)Container.DataItem)["FLDPLANINGPRIORITY"]%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblRespDisciplineHeader" runat="server">Resp Discipline</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#((DataRowView)Container.DataItem)["FLDDISCIPLINENAME"]%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:label ID="lblStatusHeader" runat="server">Status</asp:label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#((DataRowView)Container.DataItem)["FLDHARDNAME"] %>
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
