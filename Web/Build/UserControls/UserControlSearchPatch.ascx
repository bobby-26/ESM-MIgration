<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlSearchPatch.ascx.cs" Inherits="UserControls_UserControlSearchPatch" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>


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
<asp:Panel ID="pnlcover" runat="server" DefaultButton="btnsearch" CssClass="input" Width="420px" Height="16px">
        <asp:TextBox ID="txtmuticolumn" runat="server" Width="290px" Height="14px" CssClass="input" BorderStyle="None" 
        style="vertical-align:top;" BorderWidth="0px" ></asp:TextBox>        
        <asp:Button ID="btnsearch" runat="server" OnClick="btnsearch_click" Width="5px" Height="14px"/>
        <asp:TextBox ID="txtmultivalue" Visible="false" runat="server" CssClass="input" Width="05px" style="vertical-align:top" BorderStyle="None" Height="14px" />
        <asp:ImageButton ID="btnmulticolumn" runat="server" ImageUrl="<%$ PhoenixTheme:images/dropdown_mouseout.png %>"
            Width="14px" OnClick="btnmulticolumn_OnClick" ToolTip="Show All Patches" style="cursor:pointer" ImageAlign="Right"
            CommandArgument="1" BorderStyle="NotSet"  BorderWidth="1px" BorderColor="ControlLight" /></asp:Panel>
            
    <asp:Panel ID="panel1" runat="server" BackColor="White" Visible="false" Style="max-height: 200px; z-index:90000;
        overflow: scroll" Width="343px" BorderWidth="1px">
        <table width="100%" border="0" class="datagrid_pagestyle">
            <tr>
                  <td align="center">
                    <asp:TextBox ID="txtPatchName" MaxLength="10" Width="100px" runat="server" CssClass="input">
                    </asp:TextBox>
                    </td>
                    
                    <td>
                     <asp:TextBox ID="txtCreatedBy" MaxLength="10" Width="100px" runat="server" CssClass="input">
                    </asp:TextBox>
                    </td>
                    <td>
                    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                        Width="30px"></asp:Button>
                </td>
            </tr>
          </table>
        <asp:GridView ID="gvMulticolumn" runat="server" AutoGenerateColumns="False" Font-Size="11px"
            AllowSorting="true" Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false"
            OnRowDataBound="gvMulticolumn_RowDataBound" OnRowCreated="gvMulticolumn_RowCreated"
            OnSelectedIndexChanging="gvMulticolumn_SelectedIndexChanging"     OnRowCommand="gvMulticolumn_RowCommand">
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
                        <asp:LinkButton ID="lnkPatchNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDPATCHNAME"
                            ForeColor="White">Patch Name&nbsp;</asp:LinkButton>
                        <img id="FLDPATCHNAME" runat="server" visible="false" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblPatchdykey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                        <asp:LinkButton ID="lnkFileName" runat="server" CommandName="SELECT" CommandArgument="<%# Container.DataItemIndex %>"
                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENAME") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle Wrap="false" HorizontalAlign="Center" />
                    <HeaderTemplate>
                        Subject
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblSubject" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTHINT") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle Wrap="false" HorizontalAlign="Center" />
                    <HeaderTemplate>
                        Created By
                    </HeaderTemplate>
                    <ItemTemplate>                       
                        <asp:Label ID="lnkCreatedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPATCHCREATEDBY") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
</div>
