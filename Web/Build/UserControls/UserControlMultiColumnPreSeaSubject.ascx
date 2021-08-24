<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlMultiColumnPreSeaSubject.ascx.cs"
    Inherits="UserControlMultiColumnPreSeaSubject" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
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
    
    function  <%=this.ClientID %>DropMouseOver(id)
     {
        document.getElementById(id).src = '<%= Session["images"] %>' + "/dropdown_mouseover.png";
    }  
    
    function  <%=this.ClientID %>DropMouseOut(id)
     {
        document.getElementById(id).src = '<%= Session["images"] %>' + "/dropdown_mouseout.png";
    }     
</script>

<eluc:Error runat="server" Visible="false" ID="ucError" />
<div id="div1" style="width: auto; padding: 0; position: relative; height: auto">
    <asp:Panel ID="pnlcover" runat="server" DefaultButton="btnsearch" CssClass="input"
        Width="343px" Height="16px">
        <asp:TextBox ID="txtmuticolumn" runat="server" Width="290px" Height="14px" CssClass="input"
            BorderStyle="None" Style="vertical-align: top;" BorderWidth="0px"></asp:TextBox>
        <asp:Button ID="btnsearch" runat="server" OnClick="btnsearch_click" Width="5px" Height="14px" />
        <asp:TextBox ID="txtmultivalue" runat="server" CssClass="input" Width="5px" Style="vertical-align: top"
            BorderStyle="None" Height="14px" />
        <asp:ImageButton ID="btnmulticolumn" runat="server" ImageUrl="<%$ PhoenixTheme:images/dropdown_mouseout.png %>"
            Width="14px" OnClick="btnmulticolumn_OnClick" ToolTip="Show All Subjects" Style="cursor: pointer"
            ImageAlign="Right" CommandArgument="1" BorderStyle="NotSet" BorderWidth="1px"
            BorderColor="ControlLight" /></asp:Panel>
    <asp:Panel ID="panel1" runat="server" BackColor="White" Visible="false" Style="max-height: 200px;
        z-index: 90000; overflow: scroll" Width="343px" BorderWidth="1px">
        <table width="100%" border="0" class="datagrid_pagestyle">
            <tr>
                <td align="center">
                    <asp:Label ID="lblPagenumber" runat="server">
                    </asp:Label>
                    <asp:Label ID="lblPages" runat="server">
                    </asp:Label>
                    <asp:Label ID="lblRecords" runat="server">
                    </asp:Label>&nbsp;&nbsp;
                </td>
                <td align="center">
                    <asp:TextBox ID="txtnopage" MaxLength="5" Width="20px" runat="server" CssClass="input">
                    </asp:TextBox>
                    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                        Width="30px"></asp:Button>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                    &nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                    &nbsp;&nbsp; Type &nbsp;
                    <asp:DropDownList ID="ddlSubjectType" runat="server" CssClass="dropdown_mandatory"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlSubjectTypeAdd_changed">
                        <asp:ListItem Text="--Select--" Value="Dummy"></asp:ListItem>
                        <asp:ListItem Text="Theoretical" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Practical" Value="1"></asp:ListItem>
                    </asp:DropDownList>
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
                <asp:TemplateField>
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle Wrap="false" HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkCitytNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDSUBJECTNAME"
                            ForeColor="White">Subject&nbsp;</asp:LinkButton>
                        <img id="FLDSUBJECTNAME" runat="server" visible="false" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblSubjectId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTID") %>'></asp:Label>
                        <asp:LinkButton ID="lnkSubjectName" runat="server" CommandName="SELECT" CommandArgument="<%# Container.DataItemIndex %>"
                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTNAME") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>               
                <asp:TemplateField>
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle Wrap="false" HorizontalAlign="Center" />
                    <HeaderTemplate>
                        Type
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblSubjectType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBTYPENAME") %>'></asp:Label>                        
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
</div>
