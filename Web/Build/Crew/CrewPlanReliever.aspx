<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewPlanReliever.aspx.cs"
    Inherits="CrewPlanReliever" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Plan Reliever</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmVesselPosition" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <%--    <asp:UpdatePanel runat="server" ID="pnlPlanReliever">
        <ContentTemplate>     --%>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <div id="divHeading" style="vertical-align: top">
                <eluc:Title runat="server" ID="ucTitle" Text="Plan Reliever" ShowMenu="false" />
                <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            </div>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="CrewRelieverTabs" runat="server" OnTabStripCommand="CrewRelieverTabs_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
        </div>
        <table cellpadding="2" cellspacing="2" width="100%">
            <tr>
                <td>
                    <asp:Label ID="lblnote" runat="server" CssClass="guideline_text">
                        <asp:Literal ID="lblNotes" runat="server" Text="Note:These filters are applied by default in finding the suitable seafarer"></asp:Literal>
                        <br />
                        <asp:Literal ID="lbla" runat="server" Text="a)Seafarers with same vessel type experience"></asp:Literal>
                        <br />
                        <asp:Literal ID="lblb" runat="server" Text="b)Seafarers in same rank as the relivee"></asp:Literal>
                        <br />
                        <asp:Literal ID="lblc" runat="server" Text="c)Seafarers onleave"></asp:Literal>
                        <br />
                        <asp:Literal ID="lbld" runat="server" Text="c)Tooltip on rank will show the combined rank exp and combined vessel type exp"></asp:Literal>
                    </asp:Label>
                </td>
            </tr>
        </table>
        <div id="div1" style="position: relative; z-index: 0; width: 100%;">
            <b>
                <asp:Literal ID="lblCombinedExp" runat="server" Text="Combined Exp"></asp:Literal>
            </b>
            <asp:GridView ID="gvRelieverMatrix" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnRowDataBound="gvRelieverMatrix_ItemDataBound"
                ShowFooter="true" ShowHeader="true" EnableViewState="false">
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                <RowStyle Height="10px" />
                <Columns>
                    <asp:TemplateField>
                        <ItemStyle HorizontalAlign="Left" Width="40%"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Literal ID="lblName" runat="server" Text="Name of Seafarer Onboard"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="5%"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Literal ID="lblRankExp" runat="server" Text="Rank Experience"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDRANKEXP") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Literal ID="lblVesselTypeExp" runat="server" Text="Vessel Type Experience"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDVESSELTYPEEXP")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Literal ID="lblReliefDate" runat="server" Text="Relief Date"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDRELIEFDUEDATE"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <asp:TextBox ID="txt1" runat="server" Visible="false" ></asp:TextBox>
         <asp:TextBox ID="txt2" runat="server" Visible="false" ></asp:TextBox>
        <div class="navSelect" style="position: relative; width: 15px">
            <eluc:TabStrip ID="RelieverMenu" runat="server" OnTabStripCommand="RelieverMenu_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <div id="divReliever" style="position: relative; z-index: 0; width: 100%;">
            <asp:RadioButtonList ID="rblOtherRank" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblOtherRank_SelectedIndexChanged"
                RepeatDirection="Horizontal">
                <asp:ListItem Text="Current Rank" Value="0" Selected="True"></asp:ListItem>
                <asp:ListItem Text="Other Rank" Value="1"></asp:ListItem>
                <asp:ListItem Text="New Applicant" Value="2"></asp:ListItem>
            </asp:RadioButtonList>
            <asp:GridView ID="gvRelieverSearch" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                OnRowCreated="gvRelieverSearch_RowCreated" Width="100%" CellPadding="3" ShowHeader="true"
                OnRowDataBound="gvRelieverSearch_RowDataBound" OnRowCommand="gvRelieverSearch_RowCommand"
                EnableViewState="false" AllowSorting="true" OnSorting="gvRelieverSearch_Sorting">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                <RowStyle Height="10px" />
                <Columns>
                    <asp:TemplateField HeaderText="File no">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblfileno" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEMPLOYEECODE")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Off-Signer">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lblReliverHeader" runat="server" CommandName="Sort" CommandArgument="FLDEMPLOYEENAME"
                                ForeColor="White">Name&nbsp;</asp:LinkButton>
                            <img id="FLDEMPLOYEENAME" runat="server" visible="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkReliever" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'></asp:LinkButton>
                            <asp:Label ID="lblRelieverName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'></asp:Label>
                            <%--<asp:LinkButton ID="lnkEployeeName" runat="server" CommandArgument="<%#Container.DataItemIndex%>"
                                    Text='' CommandName="GETEMPLOYEE"></asp:LinkButton>--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rank">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lblEmpRankHeader" runat="server" CommandName="Sort" CommandArgument="FLDEMPLOYEERANKNAME"
                                ForeColor="White">Rank&nbsp;</asp:LinkButton>
                            <img id="FLDEMPLOYEERANKNAME" runat="server" visible="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblSuitableDoc" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUITABLEDOC") %>'></asp:Label>
                            <asp:Label ID="lblRelieverId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></asp:Label>
                            <asp:Label ID="lblPlanned" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANNED") %>'></asp:Label>
                            <asp:Label ID="lblnewappyn" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEWAPPYN") %>'></asp:Label>
                            <asp:Label ID="lblIsTop4" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISTOP4") %>'></asp:Label>
                            <asp:Label ID="lblRankid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'></asp:Label>
                            <asp:Label ID="lblEmployeeRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEERANKNAME") %>'></asp:Label>
                              <eluc:ToolTip ID="ucToolTipAddress" runat="server"  />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rank Experience">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Label ID="lblHeaderRankExperience" runat="server">Rank <br /> Experience</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDEXPERIENCE")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lblVslTypExpHeader" runat="server">Vessel <br /> Type <br /> Experience</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDVESSELTYPEEXP")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sign-off Date">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Label ID="lblHeaderSignoffDate" runat="server">Sign-off Date</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDLASTSIGNOFFDATE"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="DOA">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Label ID="lblHeaderDOA" runat="server">DOA</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDOA"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                            <asp:Label ID="lblHeaderStatus" runat="server">Status</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEMPLOYEESTATUS")+"/"+DataBinder.Eval(Container, "DataItem.FLDSTATUS")%>'
                                ToolTip='<%# DataBinder.Eval(Container, "DataItem.FLDSTATUSDESCRIPTION")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <HeaderTemplate>
                            <asp:Label ID="lblActionHeader" runat="server">Action</asp:Label>
                        </HeaderTemplate>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <asp:ImageButton ID="imgSuitableCheck" runat="server" CommandName="SUITABILITYCHECK"
                                ImageUrl="<%$ PhoenixTheme:images/crew-suitability-check.png %>" ToolTip="Suitability Check" />
                            <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                width="3" />
                            <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/plan.png %>"
                                CommandName="PLANRELIEVER" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                ToolTip="Plan Reliever"></asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <table width="100%" border="0" class="datagrid_pagestyle">
            <tr>
                <td nowrap align="center">
                    <asp:Label ID="lblRPagenumber" runat="server">
                    </asp:Label>
                    <asp:Label ID="lblRPages" runat="server">
                    </asp:Label>
                    <asp:Label ID="lblRRecords" runat="server">
                    </asp:Label>&nbsp;&nbsp;
                </td>
                <td nowrap align="left" width="50px">
                    <asp:LinkButton ID="cmdRPrevious" runat="server" OnCommand="PagerRButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                </td>
                <td width="20px">
                    &nbsp;
                </td>
                <td nowrap align="right" width="50px">
                    <asp:LinkButton ID="cmdRNext" OnCommand="PagerRButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                </td>
                <td nowrap align="center">
                    <asp:TextBox ID="txtRnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                    </asp:TextBox>
                    <asp:Button ID="btnRGo" runat="server" Text="Go" OnClick="cmdRGo_Click" CssClass="input"
                        Width="40px"></asp:Button>
                </td>
            </tr>
        </table>
        <table cellpadding="1" cellspacing="1">
            <tr>
                <td>
                    <table>
                        <tr class="rowred">
                            <td width="5px" height="10px">
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    * Required Documents is missing
                </td>
            </tr>
        </table>
        <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="btnPlan_Click" />
    </div>
    <%--  </ContentTemplate>
    </asp:UpdatePanel>--%>
    </form>
</body>
</html>
