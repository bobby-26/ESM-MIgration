<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementFormUploadRevisionList.aspx.cs" Inherits="DocumentManagementFormUploadRevisionList" %>

<%@ Import Namespace="SouthNests.Phoenix.DocumentManagement" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Form Field</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersCountry" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <div>
        <asp:UpdatePanel runat="server" ID="pnlCountryEntry">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                    width: 100%">
                    <div class="subHeader" style="position: relative">
                        <div id="divHeading">
                            <eluc:Title runat="server" ID="ucTitle" Text="Revisions" ShowMenu="false"></eluc:Title>
                            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                        </div>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="MenuDocument" runat="server" OnTabStripCommand="MenuDocument_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                    </div>
                    <div class="navSelect" style="position: relative; width: 15px">
                        <eluc:TabStrip ID="MenuRegistersCountry" runat="server" OnTabStripCommand="RegistersCountry_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                    <div id="divGrid" style="position: relative; z-index: 0">
                        <asp:GridView ID="gvFormRevision" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnRowCommand="gvFormRevision_RowCommand" OnRowDataBound="gvFormRevision_ItemDataBound"
                            OnRowCancelingEdit="gvFormRevision_RowCancelingEdit" OnRowDeleting="gvFormRevision_RowDeleting"
                            OnRowUpdating="gvFormRevision_RowUpdating" OnRowEditing="gvFormRevision_RowEditing"
                            OnRowCreated="gvFormRevision_RowCreated" ShowFooter="true" ShowHeader="true"
                            EnableViewState="false" AllowSorting="true" OnSelectedIndexChanging="gvFormRevision_SelectedIndexChanging"
                            OnSorting="gvFormRevision_Sorting" DataKeyNames="FLDFORMREVISIONID">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <Columns>
                                <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                                <asp:TemplateField HeaderText="Date">
                                    <HeaderTemplate>
                                        Added Date
                                    </HeaderTemplate>
                                    <ItemTemplate>                                        
                                        <asp:LinkButton ID="lnkAddedDate" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataItemIndex %>'
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATE") %>' Visible="false"></asp:LinkButton>
                                        <asp:Label ID="lblFormId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>'
                                            Visible="false"></asp:Label>
                                        <asp:Label ID="lblRevisionId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMREVISIONID") %>'
                                            Visible="false"></asp:Label>
                                         <asp:HyperLink ID="lnkfilename" Target="_blank" runat="server"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATE") %>'
                                        Height="14px" ToolTip="Download Form"></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Added By">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Added By
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblAddedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAUTHORNAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Revison Number">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Revision
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblVersionNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONDETAILS") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Approved Date">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Approved Date
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblApprovedYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVEDYN") %>'
                                            Visible="false"></asp:Label>
                                        <asp:Label ID="lblApprovedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVEDDATE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Approved By">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Approved By
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblApprovedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVEDBYNAME") %>'></asp:Label>
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
                                            CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                            ToolTip="Select"></asp:ImageButton>
                                        <asp:ImageButton runat="server" AlternateText="View Content" ImageUrl="<%$ PhoenixTheme:images/te_view.png %>"
                                            CommandName="VIEWCONTENT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmgViewContent"
                                            ToolTip="View Content" Visible="false"></asp:ImageButton>
                                        <img id="Img3" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                            ToolTip="Delete"></asp:ImageButton>
                                        <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" AlternateText="APPROVE" ImageUrl="<%$ PhoenixTheme:images/approve.png %>"
                                            CommandName="APPROVE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdApprove"
                                            ToolTip="Approve"></asp:ImageButton>                                                                          
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div id="divPage" style="position: relative;">
                        <table width="100%" border="0" class="datagrid_pagestyle">
                            <tr>
                                <td nowrap="nowrap" align="center">
                                    <asp:Label ID="lblPagenumber" runat="server"> </asp:Label>
                                    <asp:Label ID="lblPages" runat="server"> </asp:Label>
                                    <asp:Label ID="lblRecords" runat="server"> </asp:Label>&nbsp;&nbsp;
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
                                    <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input"> </asp:TextBox>
                                    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                        Width="40px"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </div>                  
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
