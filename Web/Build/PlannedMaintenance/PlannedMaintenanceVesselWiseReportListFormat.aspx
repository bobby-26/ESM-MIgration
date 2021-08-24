<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceVesselWiseReportListFormat.aspx.cs" 
    Inherits="PlannedMaintenance_PlannedMaintenanceVesselWiseReportListFormat" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vessel Wise Template Certificates List</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmReportCertificateList" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>

    <asp:UpdatePanel runat="server" ID="pnlTemplate">
        <ContentTemplate>
            <div style="top: 100px; margin-left: auto; margin-right: auto; width: 100%;">
                <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <div class="subHeader" style="position: relative; right: 0px">
                        <eluc:Title runat="server" ID="Title1" Text="Certificates Multiple Formats" ShowMenu="true">
                        </eluc:Title>
                    </div>
                </div>
            <div>
            <table width="100%">
                <tr>
                    <td>
                        <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtVessel" runat="server" CssClass="readonlytextbox" ReadOnly="true" Visible="false"></asp:TextBox>
                    </td>
                    <td>
                        <eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems="true" VesselsOnly="true"
                            CssClass="dropdown_mandatory"   />
                    </td>
                    <td>
                        <asp:Literal ID="lblTemplate" runat="server" Text="Template"></asp:Literal>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTemplate" runat="server" CssClass="gridinput_mandatory"
                                    DataValueField="FLDTEMPLATEID"
                                    DataTextField="FLDNAME" Width="200px"  >
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
        <div class="navSelect" style="position: relative; clear: both; width: 15px">
            <eluc:TabStrip ID="MenuDivWorkOrder" runat="server" OnTabStripCommand="MenuDivWorkOrder_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <br />
            <asp:GridView ID="gvTemplate" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                <RowStyle Height="10px" />
                <Columns>
                    <asp:TemplateField>
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label id="lblStatics" runat="server" Text="Certificate Name"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDCERTIFICATENAME")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label id="lblStatics" runat="server" Text="Issue Date"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDDATEOFISSUE")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label id="lblStatics" runat="server" Text="Expiry Date"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDDATEOFEXPIRY")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label id="lblStatics" runat="server" Text="Issued By"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDISSUINGAUTHORITYNAME")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label id="lblStatics" runat="server" Text="Due Date for Survey/Follow up"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDNEXTDUEDATE")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label id="lblStatics" runat="server" Text="Window(Range)"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDWINDOWPERIODBEFORE")%>-
                                <%# DataBinder.Eval(Container, "DataItem.FLDWINDOWPERIODAFTER")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label id="lblStatics" runat="server" Text="Type of Next Survey"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDSURVEYTYPENAME")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label id="lblStatics" runat="server" Text="Planned Date of Survey"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDPLANDATE")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label id="lblStatics" runat="server" Text="Port"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDSEAPORTNAME")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label id="lblStatics" runat="server" Text="Date of Initial Audit/Anniversary Date"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDANNIVERSARYDATE")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label id="lblStatics" runat="server" Text="Date of last Survey"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDLASTSURVEYDATE")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label id="lblStatics" runat="server" Text="Type of Last Audit/Survey"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDLASTSURVEYTYPENAME")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label id="lblStatics" runat="server" Text="Remarks"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDCERTIFICATEREMARKS")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
