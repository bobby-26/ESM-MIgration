<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewLicenceHandOver.aspx.cs"
    Inherits="CrewLicenceHandOver" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="ajaxToolkit" Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%--<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>--%>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Licence Hand Over</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixCrew.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmLicenceHandover" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <div class="navigation" id="Div1" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <div class="subHeader">
            <eluc:Title runat="server" ID="Title3" Text="Licence Hand Over" ShowMenu="true">
            </eluc:Title>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="CrewLicReq" runat="server" OnTabStripCommand="CrewLicReq_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
        </div>
        <div id="divGuidance" runat="server">
            <table id="tblGuidance">
                <tr>
                    <td>
                        <asp:Label ID="lblnote" runat="server" CssClass="guideline_text">Note: The screen  by defaults lists only 
                        requests which are in 'awaiting CRA' and 'Requisition' status .Kindly use appropriate filter to see other requests 
                        </asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="navSelect" style="position: relative; width: 15px">
            <eluc:TabStrip ID="MenuLicenceList" runat="server" OnTabStripCommand="MenuLicenceList_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
            <asp:GridView ID="gvLicReq" runat="server" AutoGenerateColumns="False" Font-Size="11px" GridLines="None"
                Width="100%" CellPadding="3" ShowHeader="true" OnRowEditing="gvLicReq_RowEditing"
                OnRowDataBound="gvLicReq_RowDataBound" EnableViewState="false" AllowSorting="true"
                OnSorting="gvLicReq_Sorting" OnRowCommand="gvLicReq_RowCommand" OnRowCreated="gvLicReq_RowCreated">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                <RowStyle Height="10px" />
                <Columns>
                   <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Literal ID="lblSrNo" runat="server" Text="S.No"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container, "DataItem.FLDROWNUMBER")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Literal ID="lblRequestNumber" runat="server" Text="Request No"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblProcessId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROCESSID")%>'></asp:Label>
                               <asp:Label ID="lblDtKey" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDDTKEY")%>'></asp:Label>
                            <asp:LinkButton ID="lnkRefno" runat="server" CommandName="SELECT" CommandArgument="<%# Container.DataItemIndex %>"
                                Text='<%#DataBinder.Eval(Container, "DataItem.FLDREFNUMBER")%>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Literal ID="lblFileNo" runat="server" Text="File No"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container, "DataItem.FLDFILENO")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lblFirstHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAME"
                                >Name</asp:LinkButton>
                            <img id="FLDNAME" runat="server" visible="false" />
                              <asp:Label ID="lblEmployeeid" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDEMPLOYEEID")%>'></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDNAME") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container, "DataItem.FLDRANKNAME")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Literal ID="lblZone" runat="server" Text="Zone"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container, "DataItem.FLDZONE")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tentative">
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container, "DataItem.FLDVESSELNAME").ToString().TrimEnd(',') %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Joined">
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container, "DataItem.FLDJOINEDVESSEL").ToString().TrimEnd(',')%>
                        </ItemTemplate>
                    </asp:TemplateField>
              <%--      <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Literal ID="lblEmpStatus" runat="server" Text="Employee Status"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container, "DataItem.FLDEMPLOYEESTATUS")%>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID= "lblCraStatus" runat="server" Text= '<%#DataBinder.Eval(Container, "DataItem.FLDCRASTATUS")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lblFlagHeader" runat="server" CommandName="Sort" CommandArgument="FLDFLAG"
                                >Flag</asp:LinkButton>
                            <img id="FLDFLAG" runat="server" visible="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblFlagId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFLAGID") %>'></asp:Label>
                            <asp:Label ID="lblFlag" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFLAGNAME") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                  <%--  <asp:TemplateField HeaderText="Consulate">
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container, "DataItem.FLDCONSULATE").ToString().TrimEnd(',')%>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lblLicenceHeader" runat="server" CommandName="Sort" CommandArgument="FLDLICENCE"
                                >Licence</asp:LinkButton>
                            <img id="FLDLICENCE" runat="server" visible="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container, "DataItem.FLDLICENCE").ToString().TrimEnd(',')%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Literal ID="lblCRANO" runat="server" Text="CRA No"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container, "DataItem.FLDCRANUMBER")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Literal ID="lblCRAExpiry" runat="server" Text="Expiry"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblDateofExpiry" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Literal ID="lblRemarks" runat="server" Text="Remarks"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container, "DataItem.FLDREMARKS")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <HeaderTemplate>
                            <asp:Literal ID="lblAction" runat="server" Text="Action"></asp:Literal>
                        </HeaderTemplate>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl='<%$ PhoenixTheme:images/te_edit.png%>'
                                CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                ToolTip="Edit"></asp:ImageButton>
                            <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                            <%-- <asp:ImageButton runat="server" AlternateText="Note" ImageUrl="<%$ PhoenixTheme:images/notepad.png %>"
                                    CommandName="NOTE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdNote"
                                    ToolTip="Note"></asp:ImageButton>--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
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
                         <eluc:MaskNumber ID="txtnopage" runat="server" Width="20px" 
                                IsInteger="true" CssClass="input" />
                        <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                            Width="40px"></asp:Button>
                    </td>
                </tr>
            </table>
        </div>
     <%--   <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />--%>
    </div>
    </form>
</body>
</html>
