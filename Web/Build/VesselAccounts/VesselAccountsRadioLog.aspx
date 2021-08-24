<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsRadioLog.aspx.cs"
    Inherits="VesselAccountsRadioLog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="../UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselCrew" Src="~/UserControls/UserControlVesselEmployee.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Radio Log</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmOtherCrew" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlRadioLog">
            <ContentTemplate>
                <div style="top: 100px; margin-left: auto; margin-right: auto; width: 100%;">
                    <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
                        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                        <div class="subHeader" style="position: relative; right: 0px">
                            <eluc:Title runat="server" ID="Title1" Text="Radio Log" ShowMenu="<%# Title1.ShowMenu %>"></eluc:Title>
                            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                        </div>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="MenuRadioLog" runat="server" OnTabStripCommand="MenuRadioLog_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <div style="min-height: 150px; width: 100%;">
                        <table cellpadding="1" cellspacing="1" width="100%">
                            <tr>
                                <td>
                                    <asp:Literal ID="lblOnAccountFor" runat="server" Text="On Account For"></asp:Literal>
                                </td>
                                <td colspan="3">
                                    <eluc:VesselCrew ID="ddlEmployee" runat="server" CssClass="input" AppendDataBoundItems="true"
                                        AppendOwnerCharterer="true" />
                                </td>
                                <td>
                                    <asp:Literal ID="lblINMARSATType" runat="server" Text="INMARSAT Type"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Hard ID="ucINMAR" AppendDataBoundItems="true" CssClass="input_mandatory" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblServiceProvider" runat="server" Text="Service Provider"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Address ID="ucServiceProvider" AppendDataBoundItems="true" CssClass="input_mandatory"
                                        runat="server" OnTextChangedEvent="ucServiceProvider_TextChanged" AutoPostBack="true"
                                        AddressType="132" />
                                </td>
                                <td>
                                    <asp:Literal ID="lblLESCode" runat="server" Text="LES Code"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Hard ID="ucLESCode" CssClass="input_mandatory" AppendDataBoundItems="true"
                                        runat="server" AutoPostBack="true" />
                                </td>
                                <td>
                                    <asp:Literal ID="lblOceanRegion" runat="server" Text="Ocean Region"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Hard ID="ucOceanRegion" CssClass="input_mandatory" AppendDataBoundItems="true"
                                        runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblChannelType" runat="server" Text="Channel Type"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Hard ID="ucChannelType" AppendDataBoundItems="true" CssClass="input_mandatory"
                                        AutoPostBack="true" OnTextChangedEvent="ucChannelType_Changed" runat="server" />
                                </td>
                                <td>
                                    <asp:Literal ID="lblCallDateandTimeUTC" runat="server" Text="Call Date and Time (UTC)"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Date ID="txtCallDate" runat="server" CssClass="input_mandatory" />
                                    <asp:TextBox ID="txtCallTime" runat="server" CssClass="input_mandatory" Width="50px" />
                                    <ajaxToolkit:MaskedEditExtender ID="txtTimeMaskEdit" runat="server" TargetControlID="txtCallTime"
                                        Mask="99:99" MaskType="Time" AcceptAMPM="false" ClearMaskOnLostFocus="false"
                                        ClearTextOnInvalid="false" />
                                </td>
                                <td>
                                    <asp:Literal ID="lblCalledNumber" runat="server" Text="Called Number"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Number ID="txtCallNumber" runat="server" CssClass="input_mandatory" MaxLength="50"
                                        Width="125px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblCalledCountry" runat="server" Text="Called Country"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Country runat="server" ID="ddlCountry" CssClass="input" AppendDataBoundItems="true" />
                                </td>
                                <td>
                                    <asp:Literal ID="lblDurationType" runat="server" Text="Duration Type"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Hard ID="ucDurationType" CssClass="input" runat="server" />
                                </td>
                                <td>
                                    <asp:Literal ID="lblCalledDuration" runat="server" Text="Called Duration"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Number ID="txtCallDurationMin" runat="server" CssClass="input_mandatory" IsInteger="true"
                                        Width="50px" MaxLength="4" AutoPostBack="true" OnTextChangedEvent="txtCallDurationMin_Changed" />
                                    &nbsp;
                                <asp:Literal ID="lblMinutes" runat="server" Text="(Minutes)"></asp:Literal>
                                    <eluc:Number ID="txtCallDurationSec" runat="server" CssClass="input_mandatory" Width="50px"
                                        MaxLength="2" AutoPostBack="true" OnTextChangedEvent="txtCallDurationSec_Changed" />
                                    &nbsp;<asp:Literal ID="lblSeconds" runat="server" Text="(Seconds)"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblChargeHours" runat="server" Text="Charge Hours"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Hard ID="ucChargeHours" AppendDataBoundItems="true" CssClass="input" runat="server" />
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtRate" runat="server" CssClass="input" Width="1px"></asp:TextBox>
                                    <asp:TextBox ID="txtRateperSec" runat="server" CssClass="input" Width="1px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Literal ID="lblTotalCharges" runat="server" Text="Total Charges"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Number ID="txtTotalCharges" runat="server" CssClass="input" ReadOnly="true" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <hr />
                    <div class="navSelect" style="position: relative; clear: both; width: 15px">
                        <eluc:TabStrip ID="MenuRadioLogGrid" runat="server" OnTabStripCommand="MenuRadioLogGrid_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <div id="divGrid" style="position: relative;">
                        <asp:GridView ID="gvRadioLog" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            GridLines="None" Width="100%" CellPadding="3" Style="margin-bottom: 0px" AllowSorting="true"
                            EnableViewState="false" OnSelectedIndexChanging="gvRadioLog_SelectedIndexChanging"
                            OnRowDeleting="gvRadioLog_RowDeleting" DataKeyNames="FLDRADIOLOGID" OnRowDataBound="gvRadioLog_RowDataBound"
                            OnSorting="gvRadioLog_Sorting">
                            <FooterStyle CssClass="datagrid_footerstyle" />
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <RowStyle Height="10px" />
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkName" runat="server" CommandName="Sort" CommandArgument="FLDNAME">On Account For&nbsp;</asp:LinkButton>
                                        <img id="FLDNAME" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTNAME") %>'></asp:Label>
                                        <asp:LinkButton ID="lnkname" runat="server" CommandName="Select" CommandArgument='<%# Container.DataItemIndex %>'
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblINMARSATType" runat="server" Text="INMARSAT Type"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRadioLogId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRADIOLOGID") %>'
                                            Visible="false"></asp:Label>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDINMARSAT")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblServiceProvider" runat="server" Text="Service Provider"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDSERVICEPROVIDER")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblLESCode" runat="server" Text="LES Code"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDLESCODE")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblOceanRegion" runat="server" Text="Ocean Region"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDOCEANREGION")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblChannelType" runat="server" Text="Channel Type"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDCHANNELTYPE")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkCallDate" runat="server" CommandName="Sort" CommandArgument="FLDCALLDATE">Call Date&nbsp;</asp:LinkButton>
                                        <img id="FLDCALLDATE" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCallDate" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCALLDATE","{0:dd/MMM/yyyy hh:mm}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblCallDuration" runat="server" Text="Call Duration"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDuration" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDURATION")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblTotalCharge" runat="server" Text="Total Charge"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotalCharges" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALCHARGES","{0:n2}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                        </asp:Label>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                            ToolTip="Delete"></asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div id="divPage" style="position: relative;">
                        <table width="100%" border="0" class="datagrid_pagestyle">
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
                                <td width="20px">&nbsp;
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
                    <eluc:Status ID="ucStatus" runat="server" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
