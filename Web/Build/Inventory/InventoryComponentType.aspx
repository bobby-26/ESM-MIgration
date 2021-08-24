<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventoryComponentType.aspx.cs"
    Inherits="InventoryComponentType" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ComponentTypeTreeView" Src="~/UserControls/UserControlTreeView.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VerticalSplitter" Src="~/UserControls/UserControlVerticalSplitter.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Location</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>
        <script type="text/javascript">
            function resizeFrame(obj) {           
                if (document.getElementById("divGrid") == null)
                    obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 30 + "px";
            }       
        </script>
    </telerik:RadCodeBlock>
</head>
<body onresize="resizeFrame(document.getElementById('ifMoreInfo'));resizeFrame(document.getElementById('divComponentType'))">
    <form id="frmPlannedMaintenanceComponentType" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlComponentType">
        <ContentTemplate>
            <div style="top: 100px; margin-left: auto; margin-right: auto; width: 100%;">
                <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <div class="subHeader" style="position: relative">
                        <eluc:Title runat="server" ID="Title1" Text="Component Type" ShowMenu="<%# Title1.ShowMenu %>">
                        </eluc:Title>
                        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                    </div>
                    <div class="navSelect" style="width: auto; float: right; margin-top: -26px">
                        <eluc:TabStrip ID="MenuComponentType" runat="server" OnTabStripCommand="MenuComponentType_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                    </div>
                    <div style="position: relative; overflow: scroll; height: 682px; width: 200px; float: left;"
                        id="divComponentType" runat="server">
                        <table cellpadding="0" cellspacing="0" style="float: left; width: 100%;">
                            <tr style="position: absolute">
                                <eluc:ComponentTypeTreeView ID="tvwComponentType" runat="server" OnSelectNodeEvent="ucTree_SelectNodeEvent" />
                                <asp:Label runat="server" ID="lblSelectedNode"></asp:Label>
                            </tr>
                        </table>
                    </div>
                    <eluc:VerticalSplitter runat="server" ID="ucVerticalSplit" TargetControlID="divComponentType" />
                    <div style="position: relative; overflow: hidden; clear: right;">
                        <iframe runat="server" id="ifMoreInfo" scrolling="no" style="min-height: 300px; width: 100%">
                        </iframe>
                        <div class="navSelect" style="position: relative; clear: both; width: 15px">
                            <eluc:TabStrip ID="MenuRegistersComponentType" runat="server" OnTabStripCommand="MenuRegistersComponentType_TabStripCommand">
                            </eluc:TabStrip>
                        </div>
                        <div id="divGrid" style="position: relative; z-index: 0; width: 100%;" runat="server">
                        <asp:GridView ID="gvComponentType" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnRowCommand="gvComponentType_RowCommand" OnRowDataBound="gvComponentType_ItemDataBound"
                            OnRowCancelingEdit="gvComponentType_RowCancelingEdit" OnRowDeleting="gvComponentType_RowDeleting"
                            OnRowEditing="gvComponentType_RowEditing" ShowHeader="true" EnableViewState="false"
                            AllowSorting="true" OnSorting="gvComponentType_Sorting" OnSelectedIndexChanging="gvComponentType_SelectedIndexChanging"
                            DataKeyNames="FLDCOMPONENTTYPEID">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <RowStyle Height="10px" />
                            <Columns>
                                <asp:TemplateField HeaderText="number">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lblnumberHeader" runat="server" CommandName="Sort" CommandArgument="FLDCOMPONENTNUMBER"
                                            ForeColor="White">Number&nbsp;</asp:LinkButton>
                                        <img id="FLDCOMPONENTNUMBER" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="StockItem Name">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lblComponentTypeNameHeader" runat="server" CommandName="Sort"
                                            CommandArgument="FLDCOMPONENTNAME" ForeColor="White">Name&nbsp;</asp:LinkButton>
                                        <img id="FLDCOMPONENTNAME" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblComponentTypeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTTYPEID") %>'></asp:Label>
                                        <asp:Label ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                        <asp:LinkButton ID="lnkStockItemName" runat="server" CommandName="Select" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTTYPEID") %>'
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Maker">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblMakerHeader" runat="server">Maker&nbsp;
                                        </asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblMaker" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.MAKERNAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Type">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lblHeaderType" runat="server" CommandName="Sort" CommandArgument="FLDTYPE"
                                            ForeColor="White">Type&nbsp;</asp:LinkButton>
                                        <img id="FLDTYPE" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PreferredVendor">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblPrefferedVendorHeader" runat="server">Preferred Vendor&nbsp;
                                        </asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblPrefferedVendor" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.VENDORNAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Class">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lblComponentClassHeader" runat="server" CommandName="Sort" CommandArgument="FLDCOMPONENTCLASSID"
                                            ForeColor="White">Class&nbsp;</asp:LinkButton>
                                        <img id="FLDCOMPONENTCLASSID" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblComponentClassName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.COMPONENTCLASSNAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                        </asp:Label>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                            ToolTip="Delete"></asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div id="divPage" style="position: relative;">
                            <table width="100%" border="0" cellpadding="1" cellspacing="1" class="datagrid_pagestyle">
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
                                        <asp:TextBox ID="txtnopage" MaxLength="5" Width="20px" runat="server" CssClass="input">
                                        </asp:TextBox>
                                        <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                            Width="40px"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        </div>
                        
                    </div>
                </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="gvComponentType" />
            <asp:AsyncPostBackTrigger ControlID="tvwComponentType" />
        </Triggers>
    </asp:UpdatePanel>
     <script type="text/javascript">
         resizeFrame(document.getElementById('ifMoreInfo')); resizeFrame(document.getElementById('divComponentType'));
    </script>
    </form>
</body>
</html>
