<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlMultiColumnDMRVoyagePort.ascx.cs" Inherits="UserControlMultiColumnDMRVoyagePort" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
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
<div id="div1" style="width: auto; padding: 0; position:relative; height:auto">
    <asp:Panel ID="pnlcover" runat="server" style="z-index: -1;" DefaultButton="btnsearch">
        <asp:TextBox ID="txtmuticolumn" runat="server" Width="330px" CssClass="input" ToolTip="For Search Press Enter Key "></asp:TextBox>
        <asp:ImageButton ID="btnmulticolumn" runat="server" ImageUrl="<%$ PhoenixTheme:images/arrowDown.png %>" Height="10px" Width="13px"
            OnClick="btnmulticolumn_OnClick" ToolTip="Show All Port" CommandArgument="1" />
        <asp:Button ID="btnsearch" runat="server" OnClick="btnsearch_click" />
        <asp:TextBox ID="txtmultiportvalue" runat="server" CssClass="input" Width="5px" />
        <asp:TextBox ID="txtmultiportcallvalue" runat="server" CssClass="input" Width="5px" />
    </asp:Panel> 
</div>
<div style="position: relative; z-index: 1;">  
    <asp:Panel ScrollBars="Both" ID="panel1" runat="server" BackColor="White" Visible="false"
        Height="200px" Width="343px" BorderWidth="1px">
        <asp:GridView ID="gvMulticolumn" runat="server" AutoGenerateColumns="False" Font-Size="11px"
            Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" OnRowDataBound="gvMulticolumn_RowDataBound"
            OnRowCreated="gvMulticolumn_RowCreated" OnRowCommand="gvMulticolumn_RowCommand">
            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
            <SelectedRowStyle CssClass="datagrid_selectedstyle" />
            <RowStyle Height="10px" />
            <Columns>    
                <asp:TemplateField>
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>                               
                      <HeaderTemplate>
                        Name
                    </HeaderTemplate>                              
                    <ItemTemplate>
                        <asp:Label ID="lblPortCallId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORTCALLID") %>'></asp:Label>
                        <asp:Label ID="lblSeaportid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTID") %>'></asp:Label>
                        <asp:LinkButton ID="lnkSeaportName" runat="server" CommandName="SELECT" CommandArgument="<%# Container.DataItemIndex %>"
                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'></asp:LinkButton>
                    </ItemTemplate>                               
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>      
</div>
