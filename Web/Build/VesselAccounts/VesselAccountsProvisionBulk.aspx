<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsProvisionBulk.aspx.cs"
    Inherits="VesselAccountsProvisionBulk" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bulk Provision</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmForwarderExcelUpload" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <%--<asp:UpdatePanel runat="server" ID="pnlProvision">
        <ContentTemplate>--%>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <div class="subHeader" style="position: relative">
                <eluc:Title runat="server" ID="Title1" Text="Stock check of Provision Items" ShowMenu="false"></eluc:Title>
            </div>
            <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                <eluc:TabStrip ID="MenuPRV" runat="server" OnTabStripCommand="MenuPRV_TabStripCommand"></eluc:TabStrip>
            </div>
            <table width="100%">
                <tr>
                    <td colspan="4">
                        <font color="blue"><b>
                            <asp:Literal ID="lblNote" runat="server" Text="Note:"></asp:Literal>
                        </b>
                            <asp:Literal ID="lblClicktheExporttoExcelIconandSavetheExcelFile" runat="server"
                                Text="Click the Export to Excel Icon and Save the Excel File"></asp:Literal>
                            <br />
                            <asp:Literal ID="lblInthatfileputtheclosingstockvalueinClosingStockColunmthensaveanduploadthefile"
                                runat="server" Text="In that file put the closing stock value in Closing Stock Colunm then save and upload the file.."></asp:Literal>
                        </font>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblNumber" runat="server" Text="Number"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtNumber" CssClass="input" MaxLength="20" Width="80px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Literal ID="lblStoreItemName" runat="server" Text="Store Item Name"></asp:Literal>
                    </td>
                    <td colspan="5">
                        <asp:TextBox runat="server" ID="txtName" CssClass="input" Width="240px" Text=""></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="8">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblClosingDate" runat="server" Text="Closing Date"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Date ID="txtClosingDate" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <asp:Literal ID="lblChooseafile" runat="server" Text="Choose a file"></asp:Literal>
                        <%--<asp:DropDownList ID="ddlClosingDate" runat="server" CssClass="input" AutoPostBack="true"
                                OnTextChanged="txtClosingDate_TextChanged" DataTextField="FLDDISPOSITIONDATENAME"
                                DataValueField="FLDDISPOSITIONDATE">
                            </asp:DropDownList>--%>
                    </td>
                    <td>
                        <asp:FileUpload ID="FileUpload" runat="server" CssClass="input" />
                        <%--Show Available Stock Items only--%>
                    </td>
                    <td></td>
                    <td>
                        <asp:Button ID="btnBulkProvision" runat="server" OnClick="btnBulkProvision_Click"
                            CssClass="input" Text="Bulk Closing" />
                        <%--Change Closing Date--%>
                    </td>
                    <td>
                        <%--<eluc:Date ID="txtChageDate" runat="server" CssClass="input" />--%>
                    </td>
                    <td>
                        <%--<asp:Button ID="btnChange" runat="server" OnClick="btnChange_Click" CssClass="input" Text="Change Closing Date" />--%>
                    </td>
                </tr>
            </table>
            <br />
            <div class="navSelect" style="position: relative; width: 15px">
                <eluc:TabStrip ID="MenuStoreItem" runat="server" OnTabStripCommand="MenuStoreItem_TabStripCommand"></eluc:TabStrip>
            </div>
            <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                <asp:GridView ID="gvStoreItem" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    GridLines="None" Width="100%" CellPadding="3" OnRowDataBound="gvStoreItem_RowDataBound"
                    OnRowEditing="gvStoreItem_RowEditing" OnRowCancelingEdit="gvStoreItem_RowCancelingEdit"
                    OnRowUpdating="gvStoreItem_RowUpdating" ShowHeader="true" ShowFooter="true" EnableViewState="false"
                    DataKeyNames="FLDSTOREITEMID">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField HeaderText="Number">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblValid" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDVALID"]%>'></asp:Label>
                                <%#((DataRowView)Container.DataItem)["FLDNUMBER"] %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDNAME"] %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Unit">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDUNITNAME"] %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Opening Stock">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDOPENINGSTOCK"]%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Purchased Stock">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDPURCHASEDSTOCK"]%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Closing Stock">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDCLOSINGSTOCK"]%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblOpeningStock" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDOPENINGSTOCK"]%>'></asp:Label>
                                <asp:Label ID="lblPurchaseQuantity" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDPURCHASEDSTOCK"]%>'></asp:Label>
                                <eluc:Number ID="txtClosingStock" runat="server" CssClass="input_mandatory" Text='<%#((DataRowView)Container.DataItem)["FLDCLOSINGSTOCK"]%>'
                                    DecimalPlace="2" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Consumption">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDCONSUMEDSTOCK"]%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblAction" runat="server" Text="Action"></asp:Literal>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <%--<img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />--%>
                                <%--<asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>--%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <table width="100%" border="0" cellpadding="1" cellspacing="1" class="datagrid_pagestyle">
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
                        <td width="20px">&nbsp;
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
            <%--<eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="LockProvision_Confirm"
                    Visible="false" />
                <eluc:Confirm ID="ucUnLockConfirm" runat="server" OnConfirmMesage="UnLockProvision_Confirm"
                    Visible="false" />--%>
        </div>
        <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
    </form>
</body>
</html>
