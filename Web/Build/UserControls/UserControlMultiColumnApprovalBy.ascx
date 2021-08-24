<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlMultiColumnApprovalBy.ascx.cs" Inherits="UserControlMultiColumnApprovalBy" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<script type="text/javascript">
    var oldgridcolor;
    function SetMouseOver(element) {
        oldgridcolor = element.style.backgroundColor;
        element.style.backgroundColor = '#bbddff';
        element.style.cursor = 'pointer';
        element.style.textDecoration = 'underline';
    }
    function SetMouseOut(element) {
        element.style.backgroundColor = oldgridcolor;
        element.style.textDecoration = 'none';
    }

</script>

<eluc:Error runat="server" Visible="false" ID="ucError" />
<div id="div1" style="width: auto; padding: 0; position: relative; height: auto;">
    <asp:Panel ID="pnlcover" runat="server" Style="z-index: -1;" DefaultButton="btnsearch">
        <asp:TextBox ID="txtmuticolumn" runat="server" Width="330px" CssClass="input" ToolTip="For Search Press Enter Key "></asp:TextBox>
        <asp:ImageButton ID="btnmulticolumn" runat="server" ImageUrl="<%$ PhoenixTheme:images/arrowDown.png %>"
            Height="10px" Width="13px" OnClick="btnmulticolumn_OnClick" ToolTip="Show All Address"
            CommandArgument="1" />
        <asp:Button ID="btnsearch" runat="server" OnClick="btnsearch_click" />
        <asp:TextBox ID="txtmultivalue" runat="server" CssClass="input" Width="5px" />
    </asp:Panel>
</div>
<div style="position: relative; z-index: 11;">
    <asp:Panel ScrollBars="Both" ID="panel1" runat="server" BackColor="White" Visible="false"
        Height="200px" Width="343px" BorderWidth="1px">
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
                <td width="20px">&nbsp;
                </td>
                <td nowrap="nowrap" align="right" width="50px">
                    <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                </td>
                <td nowrap="nowrap" align="center">
                    <asp:TextBox ID="txtnopage" MaxLength="5" Width="20px" runat="server" CssClass="input">
                    </asp:TextBox>
                    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                        Width="30px"></asp:Button>
                </td>
            </tr>
        </table>
        <asp:GridView ID="gvMulticolumn" runat="server" AutoGenerateColumns="False" Font-Size="11px"
            AllowSorting="true" Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false"
            OnRowDataBound="gvMulticolumn_RowDataBound" OnRowCreated="gvMulticolumn_RowCreated"
            OnSelectedIndexChanging="gvMulticolumn_SelectedIndexChanging" OnSorting="gvMulticolumn_Sorting"
            OnRowCommand="gvMulticolumn_RowCommand">
            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
            <SelectedRowStyle CssClass="datagrid_selectedstyle" />
            <RowStyle Height="10px" />
            <Columns>
                <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                <asp:TemplateField>
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderTemplate>
                        <asp:Label ID="lblUserHeader" runat="server">Department&nbsp;
                        </asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblUser" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERCODE") %>'></asp:Label>
                        <asp:Label ID="lblDepartment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAME") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderTemplate>
                        <asp:LinkButton ID="lblUserNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDUSERNAME">Username&nbsp;</asp:LinkButton>
                        <img id="FLDUSERNAME" runat="server" visible="false" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblUserCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERCODE") %>'></asp:Label>
                        <asp:LinkButton ID="lnkUserName" runat="server" CommandName="SELECT" CommandArgument="<%# Container.DataItemIndex %>"
                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERNAME") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderTemplate>
                        <asp:Label ID="lblFirstNameHeader" runat="server">
                                    First Name
                        </asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblFirstName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTNAME") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
</div>
