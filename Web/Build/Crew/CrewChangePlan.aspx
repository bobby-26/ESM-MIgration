<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewChangePlan.aspx.cs" Inherits="CrewChangePlan" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Query Activity</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlNTBRManager">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false" />
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="ucTitle" Text="Crew Change Plan" ShowMenu="<%# ucTitle.ShowMenu %>">
                    </eluc:Title>
                    <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="CCPMenu" runat="server" OnTabStripCommand="CCPMenu_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                </div>
                <div class="subHeader">
                    <div style="position: absolute; right: 0px">
                        <eluc:TabStrip ID="CCPSubMenu" runat="server" OnTabStripCommand="CCPSubMenu_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                </div>
                <table cellspacing="1" cellpadding="1" width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblVesselName" runat="server" Text="Vessel Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVessel" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblDateofCrewChange" runat="server" Text="Date of Crew Change"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="txtTentativeDate" runat="server" CssClass="input_mandatory" />
                        </td>
                        <td>
                            <asp:Literal ID="lblPort" runat="server" Text="Port"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Port ID="ddlPort" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" />
                        </td>
                    </tr>
                </table>
                <hr />
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuCrewChangePlan" runat="server" OnTabStripCommand="ChangePlan_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 10; width: 100%;">
                    <asp:GridView ID="gvCCPlan" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowHeader="true" OnRowDataBound="gvCCPlan_RowDataBound"
                        EnableViewState="false" AllowSorting="true" OnSorting="gvCCPlan_Sorting">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Select" Visible="false" />
                            <asp:TemplateField HeaderText="Rank">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblRankHeader" runat="server" CommandName="Sort" CommandArgument="FLDRANKNAME"
                                        ForeColor="White">Rank&nbsp;</asp:LinkButton>
                                    <img id="FLDRANKNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblEmployeeId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblRankId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblDocumentsReq" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTSREQUIRED") %>'
                                        Visible="false"></asp:Label>
                                    <%--<asp:LinkButton ID="lnkRank" runat="server" CommandArgument="<%#Container.DataItemIndex%>"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>' CommandName="EDIT"></asp:LinkButton>--%>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%-- <asp:TemplateField>
                                 <HeaderTemplate>
                                    <asp:LinkButton ID="lblVesselHeader" runat="server" CommandName="Sort" CommandArgument="FLDVESSELNAME"
                                                        ForeColor="White">Vessel&nbsp;</asp:LinkButton>
                                    <img id="FLDVESSELNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>                                     
                                    <%# DataBinder.Eval(Container, "DataItem.FLDVESSELNAME") %>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblOffSignerHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAME"
                                        ForeColor="White">Name&nbsp;</asp:LinkButton>
                                    <img id="FLDNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCrewPlanId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWPLANID") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkEmployee" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Planned Relief">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblPlannedRelief" runat="server" Text="Planned Relief"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDEXPECTEDJOINDATE", "{0:dd/MMM/yyyy}") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Planned Port">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDSEAPORTNAME")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Off-Signer">
                                <ItemTemplate>
                                    <asp:Label ID="lblOnSignerId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERID") %>'
                                        Visible="false"></asp:Label>
                                    <asp:LinkButton ID="lnkOffSigner" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERNAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Off-Signer Rank">
                                <ItemTemplate>
                                    <asp:Label ID="lblOnSignerRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERRANK") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PD Status">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblPDStatus" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPDSTATUS")%>'></asp:Label>
                                    <eluc:ToolTip ID="ucToolTipAddress" runat="server" Text='<%#HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDAPPROVALREMARKS").ToString()) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Proceed Remarks">
                                <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <asp:Label ID="lblProceedRemarks" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPLANNEDREMARKS").ToString().Length>30 ? DataBinder.Eval(Container, "DataItem.FLDPLANNEDREMARKS").ToString().Substring(0, 10) + "..." : DataBinder.Eval(Container, "DataItem.FLDPLANNEDREMARKS").ToString() %>'></asp:Label>
                                    <eluc:ToolTip ID="ucToolTipProceedRemarks" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPLANNEDREMARKS")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Relief Due">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblReliefDueHeader" runat="server" CommandName="Sort" CommandArgument="FLDRELIEFDUEDATE"
                                        ForeColor="White">Relief Due&nbsp;</asp:LinkButton>
                                    <img id="FLDRELIEFDUEDATE" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDRELIEFDUEDATE", "{0:dd/MMM/yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Crew Change Date">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDCREWCHANGEDATE", "{0:dd/MMM/yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Crew Change Port">
                                <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDCREWCHANGEPORTNAME")%>
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
                                    <asp:ImageButton ID="imgJoiningLetter" runat="server" CommandName="JOININGLETTER"
                                        ImageUrl="<%$ PhoenixTheme:images/document_view.png %>" ToolTip="Joining Letters" />
                                    <img id="Img6" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" runat="server" />
                                    <asp:ImageButton ID="imgActivity" runat="server" CommandName="ACTIVITIES" ImageUrl="<%$ PhoenixTheme:images/72.png %>"
                                        ToolTip="Activities" />
                                    <img id="Img1" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" runat="server" />
                                    <asp:ImageButton runat="server" AlternateText="Licence Request" ImageUrl="<%$ PhoenixTheme:images/initiate-licence.png %>"
                                        CommandName="LICENCEREQUEST" CommandArgument='<%# Container.DataItemIndex %>'
                                        ID="cmdEdit" ToolTip="Initiate Licence Request"></asp:ImageButton>
                                    <img id="Img3" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" runat="server" />
                                    <asp:ImageButton runat="server" AlternateText="Course Request" ImageUrl="<%$ PhoenixTheme:images/course-request.png %>"
                                        CommandName="COURSEREQUEST" CommandArgument='<%# Container.DataItemIndex %>'
                                        ID="cmdCourse" ToolTip="Initiate Course Request"></asp:ImageButton>
                                    <img id="Img2" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" runat="server" />
                                    <asp:ImageButton runat="server" AlternateText="Medical Request" ImageUrl="<%$ PhoenixTheme:images/medical-request.png %>"
                                        CommandName="MEDICALREQUEST" CommandArgument='<%# Container.DataItemIndex %>'
                                        ID="cmdMedical" ToolTip="Initiate Medical Request"></asp:ImageButton>
                                    <img id="Img5" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" runat="server" />
                                    <asp:ImageButton runat="server" AlternateText="Working Gear Issue/Request" ImageUrl="<%$ PhoenixTheme:images/workgear_issue.png %>"
                                        CommandName="WORKGEARISSUE" CommandArgument='<%# Container.DataItemIndex %>'
                                        ID="cmdWorkGear" ToolTip="Working Gear Issue/Request"></asp:ImageButton>
                                    <img id="Img4" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" runat="server" />
                                    <asp:ImageButton runat="server" AlternateText="Generate Contract" ImageUrl="<%$ PhoenixTheme:images/te_pqtes.png %>"
                                        CommandName="CONTRACT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdGenContract"
                                        ToolTip="Generate Contract"></asp:ImageButton>
                                    <img id="Img7" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton ID="imgUnAllocatedVslExp" runat="server" CommandName="UNALLOCATEDVSLEXP"
                                        ImageUrl="<%$ PhoenixTheme:images/edit-info.png %>" ToolTip="Unallocated Vessel Expense"
                                        CommandArgument="<%# Container.DataItemIndex %>" />
                                    <img id="Img8" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Checklist" CommandName="CHECKLIST"
                                        ImageUrl="<%$ PhoenixTheme:images/checklist.png %>" ID="cmdChkList" ToolTip="Checklist">
                                    </asp:ImageButton>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
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
                            <asp:Literal ID="lblDocumentsnotmappedforthevessel" runat="server" Text="* Documents not mapped for the vessel"></asp:Literal>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
