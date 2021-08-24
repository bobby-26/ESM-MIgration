<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaCourseFees.aspx.cs"
    Inherits="PreSeaCourseFees" %>

<%@ Import Namespace="SouthNests.Phoenix.PreSea" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlPreSeaCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fees" Src="~/UserControls/UserControlPreSeaFees.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pre Sea Fees</title>
     <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
</telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPreSeaFees" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPreSeaFees">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Course Master"></eluc:Title>
                    </div>
                </div>
                <div style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuCourseMaster" runat="server" OnTabStripCommand="CourseMaster_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <div id="divSearch">
                    <table id="tblSearch" width="100%">
                        <tr>
                            <td>
                                Course Name
                            </td>
                            <td>
                                <eluc:Course ID="ddlCourse" runat="server" AppendDataBoundItems="true" CssClass="input" />
                            </td>
                            <td>
                                Fees Name
                            </td>
                            <td>
                                <asp:TextBox ID="txtFeesName" runat="server" CssClass="input"></asp:TextBox>
                            </td>
                            <td>
                                Pay term
                            </td>
                            <td>
                                <eluc:Quick runat="server" ID="ucPayterm" AppendDataBoundItems="true" CssClass="input"
                                    QuickTypeCode="94" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuPreSeaFees" runat="server" OnTabStripCommand="PreSeaFees_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvPreSeaFees" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowFooter="true" EnableViewState="true" OnRowCancelingEdit="gvPreSeaFees_RowCancelingEdit"
                        OnRowCommand="gvPreSeaFees_RowCommand" OnRowDataBound="gvPreSeaFees_RowDataBound"
                        OnRowDeleting="gvPreSeaFees_RowDeleting" OnRowEditing="gvPreSeaFees_RowEditing"
                        OnRowUpdating="gvPreSeaFees_RowUpdating">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField HeaderText="Fees Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Fees Name
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="LblCourseFeesId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSEFEEID") %>'></asp:Label>
                                    <asp:Label ID="LblCourseId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRESEACOURSEID") %>'></asp:Label>
                                    <asp:Label ID="lblFeesId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFEESID") %>'></asp:Label>
                                    <asp:Label ID="lblFeesName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFEESNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="LblCourseFeesIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSEFEEID") %>'></asp:Label>
                                    <eluc:Fees ID="ddlFeesEdit" runat="server" AppendDataBoundItems="true" CssClass="input" PreSeaFeesList='<%# PhoenixPreSeaFees.ListPreSeaFees(null) %>'
                                        SelectedFees='<%# DataBinder.Eval(Container,"DataItem.FLDFEESID") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Fees ID="ddlFeesAdd" runat="server" AppendDataBoundItems="true" PreSeaFeesList='<%# PhoenixPreSeaFees.ListPreSeaFees(null) %>' CssClass="input_mandatory" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amount">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Amount
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number runat="server" ID="ucAmountEdit" CssClass="gridinput" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'
                                        Mask="99999999.99 " />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number runat="server" ID="ucAmountAdd" CssClass="gridinput" Mask="99999999.99" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Pay Term">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Pay Term
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPayTerm" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUICKNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Quick runat="server" ID="ucPaytermEdit" AppendDataBoundItems="true" CssClass="gridinput"
                                        QuickTypeCode="94" QuickList='<%# PhoenixRegistersQuick.ListQuick(1, 94) %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Quick runat="server" ID="ucPaytermAdd" AppendDataBoundItems="true" CssClass="gridinput"
                                        QuickTypeCode="94" QuickList='<%# PhoenixRegistersQuick.ListQuick(1, 94) %>'/>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Waiver Amount
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblWaiverAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAIVERAMOUNT") %>'></asp:Label>
                                </ItemTemplate>
                                 <EditItemTemplate>
                                    <eluc:Number runat="server" ID="ucWaiverAmountEdit" CssClass="gridinput" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAIVERAMOUNT") %>'
                                        Mask="99999999.99 " />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number runat="server" ID="ucWaiverAmountAdd" CssClass="gridinput" Mask="99999999.99" />
                                </FooterTemplate>
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
                                        ToolTip="Edit" Visible="false"></asp:ImageButton>
                                    <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" visible="false" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="SAVE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save" Visible="false"></asp:ImageButton>
                                    <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="CANCEL" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel" Visible="false"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="ADD" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage" style="position: relative;">
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
