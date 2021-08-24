<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOCIMFDetail.aspx.cs" Inherits="CrewOCIMFDetail" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Batch List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewBatchList" runat="server" submitdisabledcontrols="true">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlBatch">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <div class="subHeader" style="position: relative">
                        <div id="divHeading" style="vertical-align: top">
                            <eluc:Title runat="server" ID="ucTitle" Text="OCIMF" ShowMenu="false" />
                        </div>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="OcimfLogin" runat="server" OnTabStripCommand="OcimfLoginTabs_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                    </div>
                     <div class="subHeader" style="position: relative;">
                    <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                        <eluc:TabStrip ID="Menuocimf" runat="server" OnTabStripCommand="Menuocimf_TabStripCommand"  TabStrip="false">
                        </eluc:TabStrip>
                    </span>
                   </div>
                    <br />
                    <table cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Vessel ID="lstVessel" runat="server" AppendDataBoundItems="true" CssClass="input" OnTextChangedEvent="lstVessel_OnTextChanged"
                                    AutoPostBack="true" VesselsOnly="true" Enabled="false" />
                            </td>
                            <td>
                                <asp:Literal ID="lblOilMajor" runat="server" Text="Oil Major"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Hard ID="ddlOilMajor" runat="server" HardTypeCode="100" AppendDataBoundItems="true" CssClass="dropdown_mandatory" AutoPostBack="true" />
                            </td>
                            <td>
                                <asp:Literal ID="lblPrinciple" runat="server" Text="Principle" Visible="false"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Address ID="ddlPrinciple" AddressType="128" runat="server" AppendDataBoundItems="true" Visible="false"
                                    CssClass="input" />
                            </td>
                         
                        </tr>
                    </table>
                    <div class="navSelect" style="position: relative; width: 15px">
                        <eluc:TabStrip ID="MenuCrewOCIMF" runat="server" Visible="false" OnTabStripCommand="CrewOCIMF_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <br />
                    <asp:GridView ID="gvOCIMF" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvOCIMF_RowCommand" OnRowDataBound="gvOCIMF_RowDataBound"
                        ShowFooter="false" ShowHeader="true" EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="Rank">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></asp:Label>
                                    <asp:Label ID="lblRankName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANK") %>'></asp:Label>
                                    <asp:Label ID="lblCrewType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWTYPE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nationality">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblNationality" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDNATIONALITY")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cert. Comp.">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblCertComp" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCERTIFICATECOMPETENCY")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Issuing Country">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblIssuingCountry" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDISSUINGCOUNTRY")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Admin. Accept">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblAdminAccpt" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDADMINACCEPT")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tanker Cert.">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblTankerCert" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTANKERCERTIFICATION")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="STCW V Para.">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblStcwPara" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSTCWPARA")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Radio Qual.">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblRadioQual" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDRADIOQUALIFIED")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Operator">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblYrsOperator" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDYRSWITHOPERATOR")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rank">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblYrsRank" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDYRSRANKEXPERIENCE")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tanker Type">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblYrsTankerType" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDYRSINTANKER")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="All Types">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblYrsAllType" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDYRSINALLTANKER")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Moths Tour">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDMONTHSTOUR")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Years Watch">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblYearsWatch" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDYEARSWATCH")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sign-On Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblJoinDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDSIGNONDATE", "{0:dd/MMM/yyyy}")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="English prof">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblEngProf" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDENGLISHPROF")%>'></asp:Label>
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
                                    <asp:ImageButton runat="server" AlternateText="Add to Sire" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="Add" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAdd"
                                        ToolTip="Add to SIRE"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                    <br />
                    <b>Sire Data</b>
                    <asp:GridView ID="gvSire" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowEditing="gvSire_RowEditing" OnRowCancelingEdit="gvSire_RowCancelingEdit"
                        OnRowUpdating="gvSire_RowUpdating" OnRowDeleting="gvSire_RowDeleting" OnRowDataBound="gvSire_RowDataBound"
                        ShowFooter="false" ShowHeader="true" EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Scheduled
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="20px" />
                                <ItemTemplate>
                                    <asp:Image ID="ImgUpdated" ImageUrl="<%$ PhoenixTheme:images/green-symbol.png %>" ImageAlign="AbsMiddle" Text=".." runat="server" Visible="false" />
                                    <asp:Image ID="ImgDeleted" ImageUrl="<%$ PhoenixTheme:images/highPriority.png %>" ImageAlign="AbsMiddle" Text=".." runat="server" Visible="false" />
                                    <%--    <asp:ImageButton ID="ImgDeleted" runat="server" CommandArgument="<%# Container.DataItemIndex %>"
                                        ImageUrl="<%$ PhoenixTheme:images/highPriority.png %>" ImageAlign="AbsMiddle" Text=".."
                                        ToolTip="Delete Scheduled" Visible="false" />--%>
                                    <asp:Label ID="lblUpdated" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDUPDATEDYN")%>'></asp:Label>
                                    <asp:Label ID="lblDeleted" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDELETEDYN")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Last Export">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblExportDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDLASTEXPORTDATE", "{0:dd/MMM/yyyy}")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rank">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDRANK")%>'></asp:Label>
                                    <asp:Label ID="lblCrewType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCREWTYPE")%>'></asp:Label>
                                    <asp:Label ID="lblCrewkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCREWKEY")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblRankEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDRANK")%>'></asp:Label>
                                    <asp:Label ID="lblCrewTypeEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCREWTYPE")%>'></asp:Label>
                                    <asp:Label ID="lblCrewkeyEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCREWKEY")%>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nationality">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblNationality" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDNATIONALITY")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblNationalityEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDNATIONALITY")%>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cert. Comp.">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblCertComp" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCERTCOMP")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblCertCompEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCERTCOMP")%>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Issuing Country">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblIssuingCountry" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDISSUECOUNTRY")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblIssuingCountryEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDISSUECOUNTRY")%>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Admin. Accept">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblAdminAccpt" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDADMINACCEPT")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblAdminAccptEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDADMINACCEPT")%>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tanker Cert.">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblTankerCert" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTANKERCERT")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblTankerCertEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTANKERCERT")%>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="STCW V Para.">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblStcwPara" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSTCWVPARA")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblStcwParaEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSTCWVPARA")%>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Radio Qual.">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblRadioQual" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDRADIOQUAL")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblRadioQualEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDRADIOQUAL")%>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Operator">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblYrsOperator" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDYEARSOPERATOR")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="txtYrsOperator" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.FLDYEARSOPERATOR")%>' DecimalPlace="1" IsPositive="true" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rank">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblYrsRank" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDYEARSRANK")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="txtYrsRank" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.FLDYEARSRANK")%>' DecimalPlace="1" IsPositive="true" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tanker Type">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblYrsTankerType" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDYEARSTANKERTYPE")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="txtYrsTankerType" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.FLDYEARSTANKERTYPE")%>' DecimalPlace="1" IsPositive="true" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="All Types">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblYrsAllType" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDYEARSALLTANKERTYPES")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="txtYrsAllType" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.FLDYEARSALLTANKERTYPES")%>' DecimalPlace="1" IsPositive="true" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Years Watch">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblYrsWatch" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDYEARSWATCH")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="txtYrsWatch" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.FLDYEARSWATCH")%>' DecimalPlace="1" IsPositive="true" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sign-On Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblJoinDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATEJOINEDVESSEL", "{0:dd/MMM/yyyy}")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="txtJoinDate" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDATEJOINEDVESSEL")%>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="English prof">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblEngProf" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDENGLISHPROF")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEngProf" runat="server" CssClass="gridinput" Text='<%# DataBinder.Eval(Container, "DataItem.FLDENGLISHPROF")%>'></asp:TextBox>
                                </EditItemTemplate>
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
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <eluc:Status runat="server" ID="ucStatus" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
