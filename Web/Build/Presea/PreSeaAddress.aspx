<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaAddress.aspx.cs" Inherits="PreSeaAddress" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Address List</title>
     <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript">
     function CheckAll(chkAll)
     {
        var gv = document.getElementById("<%=gvAddress.ClientID %>");
        for(i = 1;i < gv.rows.length; i++)
        {
            gv.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = chkAll.checked;
        }
     }

    </script>
</telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlAddressEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="div1">
                        <eluc:Title runat="server" ID="ucTitle" Text="Address" />
                        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuOffice" runat="server" OnTabStripCommand="MenuOffice_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative;">
                    <asp:GridView ID="gvAddress" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        Font-Size="11px" OnRowCommand="gvAddress_RowCommand" OnRowDataBound="gvAddress_RowDataBound"
                        OnRowDeleting="gvAddress_RowDeleting" AllowSorting="true" OnSorting="gvAddress_Sorting"
                        ShowFooter="false" ShowHeader="true" Width="100%">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField CommandName="Edit" Text="DoubleClick" Visible="false" />
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Center" Wrap="False" Width="60px" />
                                <HeaderStyle HorizontalAlign="Left" Wrap="true" />
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkAll" runat="server" Text="All&nbsp;&nbsp;" TextAlign="Left"
                                        onclick="CheckAll(this)" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkItem" runat="server" />
                                    <asp:Label ID="lblAddressCode" runat="server" Text='<%# Bind("FLDADDRESSCODE") %>'
                                        Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkCodeHeader" runat="server" CommandName="Sort" CommandArgument="FLDCODE"
                                        ForeColor="White">Code&nbsp;</asp:LinkButton>
                                    <img id="FLDCODE" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAddressType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESSTYPE") %>'></asp:Label>
                                    <asp:Label ID="lblCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>'></asp:Label>
                                    <asp:Label ID="lblIsBlacklisted" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISBLACKLISTED") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblAddressNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAME"
                                        ForeColor="White">Name&nbsp;</asp:LinkButton>
                                    <img id="FLDNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkAddressName" runat="server" CommandArgument='<%# Bind("FLDADDRESSCODE") %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton>
                                    <eluc:ToolTip ID="ucToolTipAddress" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESS1")+"<br>"+ DataBinder.Eval(Container,"DataItem.FLDADDRESS2")+"<br>"+ DataBinder.Eval(Container,"DataItem.FLDADDRESS3") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblPhone1Header" runat="server">Phone 1  </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPhone1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPHONE1") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblCityHeader" runat="server"> City </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITYNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkStateHeader" runat="server" CommandName="Sort" CommandArgument="FLDSTATE"
                                        ForeColor="White">STATE&nbsp;</asp:LinkButton>
                                    <img id="FLDSTATE" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="LblState" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATE") %>'></asp:Label>
                                    <asp:Label ID="lblStateName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATENAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkStatusHeader" runat="server" CommandName="Sort" CommandArgument="FLDHARDNAME"
                                        ForeColor="White">Status&nbsp;</asp:LinkButton>
                                    <img id="FLDHARDNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblDepartmentHeader" runat="server">Department</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDepartmentName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDEPARTMENTNAME").ToString().Length > 15 ? DataBinder.Eval(Container, "DataItem.FLDDEPARTMENTNAME").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDDEPARTMENTNAME").ToString() %>'></asp:Label>
                                    <eluc:ToolTip ID="ucDepartmentTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAME") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="false" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="GETADDRESS" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/Email.png %>"
                                        CommandName="SendMail" CommandArgument='<%# Bind("FLDADDRESSCODE") %>' ID="cmdEmail"
                                        ToolTip="Send Mail"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Send Verification Letter" ImageUrl="<%$ PhoenixTheme:images/task-list.png %>"
                                        CommandName="VERIFICATION" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdVerification"
                                        ToolTip="Send Verification Letter"></asp:ImageButton>
                                    <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
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
                            <td nowrap="nowrap" align="center">
                                <asp:Label ID="lblPagenumber" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPages" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecords" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap="nowrap" align="left" width="50px">
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev"><< Prev </asp:LinkButton>
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
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
