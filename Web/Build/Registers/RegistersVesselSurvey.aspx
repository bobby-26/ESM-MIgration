<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersVesselSurvey.aspx.cs"
    Inherits="Registers_RegistersVesselSurvey" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Survey</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div runat="server" id="dvscriptsk">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersSurvey" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlSurvey">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Survey"></eluc:Title>
                        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                </div>
                <div id="divControls">
                    <table width="100%" cellspacing="15">
                        <tr>
                            <td>
                                <asp:Literal ID="lblSurvey" runat="server" Text="Survey"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSurvey" runat="server" CssClass="input" Width="250px" MaxLength="500"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblSurveyType" runat="server" Text="Survey Type"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSurveyType" runat="server" CssClass="input" Width="100px">
                                </asp:DropDownList>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Panel ID="pnlperiod" runat="server" GroupingText="Commence Date" Width="400px">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lblFromDate" Text="From Date" runat="server"></asp:Literal>
                                            </td>
                                            <td>
                                                <eluc:Date ID="ucFromDate" runat="server" CssClass="input" />
                                            </td>
                                            <td>
                                                <asp:Literal ID="lblToDate" Text="To Date" runat="server"></asp:Literal>
                                            </td>
                                            <td>
                                                <eluc:Date ID="ucToDate" runat="server" CssClass="input" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="input" Width="100px">
                                    <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Intiated" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Not Intiated" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuRegistersSurvey" runat="server" OnTabStripCommand="MenuRegistersSurvey_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvSurvey" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        OnRowCreated="gvSurvey_RowCreated" Width="100%" CellPadding="3" OnRowCommand="gvSurvey_RowCommand"
                        OnRowDataBound="gvSurvey_ItemDataBound" ShowHeader="true" EnableViewState="false"
                        AllowSorting="true" OnSorting="gvSurvey_Sorting" OnRowDeleting="gvSurvey_RowDeleting">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField HeaderText="Vessel">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblVesselNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDVESSELNAME"
                                        ForeColor="White">Vessel&nbsp;</asp:LinkButton>
                                    <img id="FLDVESSELNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></asp:Label>
                                    <asp:Label ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Survey">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="250px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblHeaderSurveyName" runat="server" CommandName="Sort" CommandArgument="FLDSURVEYNAME"
                                        ForeColor="White">Survey&nbsp;</asp:LinkButton>
                                    <img id="FLDSURVEYNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSurveyId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSURVEYID") %>'></asp:Label>
                                    <asp:Label ID="lblSurveyName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSURVEYNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Survey Type">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblHeaderSurveyType" runat="server" CommandName="Sort" CommandArgument="FLDSURVEYTYPENAME"
                                        ForeColor="White">Survey Type&nbsp;</asp:LinkButton>
                                    <img id="FLDSURVEYTYPENAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSurveyTypeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSURVEYTYPEID") %>'></asp:Label>
                                    <asp:Label ID="lblSurveyTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSURVEYTYPENAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="Certificates">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblCategoryListHeader" runat="server" Text="Certificates"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCategoryList" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATECATEGORYLIST") %>'
                                        Visible="false"></asp:Label>
                                    <img id="imgCategoryList" runat="server" src="<%$ PhoenixTheme:images/te_view.png %>"
                                        onmousedown="javascript:closeMoreInformation()" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Frequency (Months)">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblFrequencyHeader" runat="server" Text="Frequency(Months)"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblFrequency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFREQUENCY") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Commence Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblCommencementDateHeader" runat="server" Text="Commence Date"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCommencementDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDCOMMENCEMENTDATE"))%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Window period (Before/After Days)">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblWindowPeriodHeader" runat="server" Text="Window period (Before/After Months)"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblWindowPeriod" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWINDOWPERIOD") %>'></asp:Label>/
                                    <asp:Label ID="lblPlusMinus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLUSORMINUSPERIOD") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Schedule Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblScheduleDateHeader" runat="server" Text="Schedule Date"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblScheduleDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDSCHEDULEDATE")) %>'></asp:Label>
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
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="cmdEdit" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="delete" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                    <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Certificates List" ImageUrl="<%$ PhoenixTheme:images/checklist.png %>"
                                        CommandName="CERTIFICATE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCertificate"
                                        ToolTip="Certificates List" Visible="false"></asp:ImageButton>
                                    <img id="Img3" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" visible="false" />
                                    <asp:ImageButton runat="server" AlternateText="Initiate Survey" ImageUrl="<%$ PhoenixTheme:images/te_pqtes.png %>"
                                        CommandName="INITIATESURVEY" CommandArgument='<%# Container.DataItemIndex %>'
                                        ID="cmdInitiateSurvey" ToolTip="Initiate Survey"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
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
                            <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                            </asp:TextBox>
                            <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                Width="40px"></asp:Button>
                        </td>
                    </tr>
                    <eluc:Status runat="server" ID="ucStatus" />
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
