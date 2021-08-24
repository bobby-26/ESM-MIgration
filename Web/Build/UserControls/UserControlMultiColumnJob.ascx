<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlMultiColumnJob.ascx.cs"
    Inherits="UserControlMultiColumnJob" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<script type="text/javascript">
    var oldgridcolor;
    function SetMouseOver(element) 
    {
        oldgridcolor = element.style.backgroundColor;
        element.style.backgroundColor = '#bbddff';
        element.style.cursor = 'pointer';
        element.style.textDecoration = 'underline';
    }
    function SetMouseOut(element)
     {
        element.style.backgroundColor = oldgridcolor;
        element.style.textDecoration = 'none';
    } 
    
</script>

<eluc:Error runat="server" Visible="false" ID="ucError" />
<div id="div1" style="width: auto; padding: 0; position: relative; height: auto">
    <asp:Panel ID="pnlcover" runat="server" Style="z-index: -1;" DefaultButton="btnsearch">
        <asp:TextBox ID="txtmuticolumn" runat="server" Width="330px" CssClass="input" ToolTip="For Search Press Enter Key "></asp:TextBox>
        <asp:ImageButton ID="btnmulticolumn" runat="server" ImageUrl="<%$ PhoenixTheme:images/arrowDown.png %>"
            Height="10px" Width="13px" OnClick="btnmulticolumn_OnClick" ToolTip="Show All Job"
            CommandArgument="1" />
        <asp:Button ID="btnsearch" runat="server" OnClick="btnsearch_click" />
        <asp:TextBox ID="txtmultivalue" runat="server" CssClass="input" Width="5px" /></asp:Panel>
</div>
<div style="position: relative; z-index: 1;">
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
                <td width="20px">
                    &nbsp;
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
            <tr>
                    <td>
                        <asp:Literal ID="lblJobCode" runat="server" Text="Job Code"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtJobCode" CssClass="input" MaxLength="20"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Literal ID="lblJobTitle" runat="server" Text="Job Title"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtJobTitle" CssClass="input" MaxLength="200" Width="240px"></asp:TextBox>
                    </td>
                <td></td>
            </tr>
            <tr>
                    <td>
                        <asp:Literal ID="lblJobClass" runat="server" Text="JJob Class"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Quick ID="ucJobClass" runat="server" CssClass="input" QuickTypeCode="34" AppendDataBoundItems="true" />
                    </td>
                <td></td>
                <td></td>
                <td></td>
            
            </tr>
        </table>
        <asp:GridView ID="gvMulticolumn" runat="server" AutoGenerateColumns="False" Font-Size="11px"
            AllowSorting="true" Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false"
            OnRowDataBound="gvMulticolumn_RowDataBound" OnRowCommand="gvMulticolumn_RowCommand"
            OnSelectedIndexChanging="gvMulticolumn_SelectedIndexChanging" OnSorting="gvMulticolumn_Sorting">
            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
            <SelectedRowStyle CssClass="datagrid_selectedstyle" />
            <RowStyle Height="10px" />
            <Columns>
                <asp:TemplateField HeaderText="Code">
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkJobCode" runat="server" CommandName="Sort" CommandArgument="FLDJOBCODE"
                            ForeColor="White">Code&nbsp;</asp:LinkButton>
                        <img id="FLDJOBCODE" runat="server" visible="false" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lbljobid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBID") %>'></asp:Label>
                        <asp:Label ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                        <asp:LinkButton ID="lnkcode" runat="server" CommandName="SELECT" CommandArgument="<%# Container.DataItemIndex %>"
                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBCODE") %>'></asp:LinkButton>
                        <%--<asp:LinkButton ID="lnkcode" runat="server" CommandName="Select" CommandArgument='<%# Container.DataItemIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBCODE") %>'></asp:LinkButton>--%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Job Title">
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkJobTitle" runat="server" CommandName="Sort" CommandArgument="FLDJOBTITLE"
                            ForeColor="White">Title&nbsp;</asp:LinkButton>
                        <img id="FLDJOBTITLE" runat="server" visible="false" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBTITLE") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Job Class">
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkJobClass" runat="server" CommandName="Sort" CommandArgument="FLDCLASS"
                            ForeColor="White">Job Class&nbsp;</asp:LinkButton>
                        <img id="FLDCLASS" runat="server" visible="false" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblClass" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLASS") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
</div>
