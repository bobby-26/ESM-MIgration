<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersAdminAssetHardwareAssembly.aspx.cs" Inherits="Registers_RegistersAdminAssetHardwareAssembly" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Asset Hardware</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="ds" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersAdminAsset" autocomplete="off" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlAdminAssetEntry">
        <ContentTemplate>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Hardware" />
                    </div>
                </div>
                <div style="top: 0px; right: 2px; position: absolute">
                    <eluc:TabStrip ID="MenuAsset" runat="server" OnTabStripCommand="Asset_TabStripCommand" 
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <div class="subHeader" style="top: 1px; right: 0px; position: relative">
                    <div id="divHeadings">
                        <eluc:Title runat="server" ID="Title1" Text="Assembly" ShowMenu="false" />
                    </div>
                </div>
                <div class="Header">
                    <div class="navSelect" style="top: 28.4px; right: 0px; position: absolute;">
                        <eluc:TabStrip ID="MenuAdminAssetAdd" runat="server" OnTabStripCommand="MenuAdminAssetAdd_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                </div>
                <eluc:Confirm ID="ucConfirm" runat="server" Visible="false" OnConfirmMesage="ucConfirm_ConfirmMesage" OKText="Yes" CancelText="No" />
                <br />
                <div>
                    <table width="100%">
                    <tr>
                        <td width="8%">
                            <asp:Literal ID="lblAssetType" runat="server" Text="Asset"></asp:Literal>
                        </td>
                        <td width="25.4%">
                            <asp:DropDownList ID="ddlAssetType" runat="server" CssClass="input_mandatory" Width="150px"
                                DataValueField="FLDASSETTYPEID" DataTextField="FLDNAME" OnSelectedIndexChanged="ddlAssetType_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td width="8%">
                            <asp:Literal ID="lblServiceTag" runat="server" Text="Serial Number"></asp:Literal>  
                        </td>
                        <td width="25.4%">
                            <asp:TextBox ID="txtSerialNo" runat="server" MaxLength="50" CssClass="input_mandatory"></asp:TextBox>
                        </td>
                        <td width="8%">
                            <asp:Literal ID="lblBudgetYear" runat="server" Text="Budget Year"></asp:Literal>
                        </td>
                        <td width="25.4%">
                            <asp:DropDownList ID="ddlYear" runat="server" CssClass="input" Width="70px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td width="8%">
                            <asp:Literal ID="lblAssetName" runat="server" Text="Name"></asp:Literal> 
                        </td>
                        <td width="25.4%">
                            <asp:TextBox ID="txtAssetName" runat="server" MaxLength="100" CssClass="input_mandatory"></asp:TextBox>
                        </td>
                        <td width="8%">
                            <asp:Literal ID="lblLocation" runat="server" Text="Zone"></asp:Literal> 
                        </td>
                        <td width="25.4%">
                            <eluc:Zone ID="ddlLocation" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" Width="120px" />
                        </td>
                        <td width="8%">
                            <asp:Literal ID="lblExpiryDate" runat="server" Text="Expiry Date"></asp:Literal> 
                        </td>
                        <td width="25.4%">
                            <eluc:UserControlDate ID="UcExpirydate" DatePicker="true" runat="server"
                                CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td width="8%">
                            <asp:Literal ID="lblTagNumber" runat="server" Text="Tag Number"></asp:Literal>  
                        </td>
                        <td width="25.4%">
                            <asp:TextBox ID="txtTagNumber" runat="server" MaxLength="50" CssClass="input"></asp:TextBox>
                        </td>
                        <td width="8%">
                            <asp:Literal ID="lblPoReference" runat="server" Text="PO Reference"></asp:Literal>
                        </td>
                        <td width="25.4%">
                            <asp:TextBox runat="server" ID="TxtPoreference" MaxLength="100" CssClass="input" Width="120px"></asp:TextBox>
                        </td>
                        <td width="8%">
                            <asp:Literal ID="lblDisposalDate" runat="server" Text="Disposal Date"></asp:Literal>
                        </td>
                        <td width="25.4%">
                            <eluc:UserControlDate ID="ucDisposalDate" runat="server" DatePicker="true"
                                CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td width="8%">
                            <asp:Literal ID="lblMaker" runat="server" Text="Maker"></asp:Literal>  
                        </td>
                        <td width="25.4%">
                            <asp:TextBox ID="txtMaker" runat="server" MaxLength="100" CssClass="input_mandatory"></asp:TextBox>
                        </td>
                        <td width="8%">
                            <asp:Literal ID="lblInvoiceNo" runat="server" Text="Invoice No"></asp:Literal>
                        </td>
                        <td width="25.4%">
                            <asp:TextBox runat="server" ID="TxtInvoiceno" MaxLength="100" Width="120px" CssClass="input"></asp:TextBox>
                        </td>
                        <td width="8%">
                            <asp:Literal ID="lblDisposalReason" runat="server" Text="Disposal Reason"></asp:Literal>
                        </td>
                        <td width="25.4%">
                            <asp:TextBox runat="server" ID="txtDisposalReason" CssClass="input" Width="250px" height="50px"
                                TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="8%">
                            <asp:Literal ID="lblModel" runat="server" Text="Model"></asp:Literal> 
                        </td>
                        <td width="25.4%">
                            <asp:TextBox ID="txtModel" runat="server" MaxLength="100" CssClass="input"></asp:TextBox>
                        </td>
                        <td width="8%">
                            <asp:Literal ID="lblInvoiceDate" runat="server" Text="Invoice Date"></asp:Literal>
                        </td>
                        <td width="25.4%">
                            <eluc:UserControlDate ID="UcInvoiceDate" runat="server" DatePicker="true"
                                CssClass="input" />
                        </td>
                        <td width="8%">
                            <asp:Literal ID="lblRemarks" runat="server" Text="Remarks"></asp:Literal>
                        </td>
                        <td width="25.4%">
                            <asp:TextBox runat="server" ID="txtRemarks" CssClass="input" Width="250px" height="50px"
                                TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="8%">
                            <asp:Literal ID="lblAssetDescription" runat="server" Text="Description"></asp:Literal> 
                        </td>
                        <td width="25.4%">
                            <asp:TextBox ID="Txtdescriptionadd" runat="server" CssClass="input" Width="250px" height="50px" TextMode="MultiLine"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
                    <br />
                </div>
                <hr style="margin:0px;padding:0px;" />
                <div style="position: relative; top:0px; overflow: hidden; clear:right;">
                    <eluc:TabStrip ID="MenuRegistersAdminAsset" runat="server" OnTabStripCommand="MenuRegistersAdminAsset_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 1">
                    <asp:GridView ID="gvAdminAsset" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvAdminAsset_RowCommand" OnRowDataBound="gvAdminAsset_RowDataBound"
                        OnRowDeleting="gvAdminAsset_RowDeleting" OnRowEditing="gvAdminAsset_RowEditing"
                        OnRowCancelingEdit="gvAdminAsset_RowCancelingEdit" OnRowUpdating="gvAdminAsset_Rowupdating"
                        ShowFooter="false" ShowHeader="true" OnSorting="gvAdminAsset_Sorting" AllowSorting="true" EnableViewState="false" DataKeyNames="FLDASSETID">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField FooterText="New Team Member">
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="19%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:ImageButton runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click"
                                        CommandName="BindData" ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" CommandArgument="1" />
                                    <asp:LinkButton ID="lblAdminAssetNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAME"
                                        ForeColor="White">Name&nbsp;</asp:LinkButton>
                                    <img id="FLDNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAdminAssetID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASSETID") %>'></asp:Label>
                                    <asp:Label ID="lblTypeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkAdminAssetName" runat="server" CommandName="SELECT" CommandArgument="<%# Container.DataItemIndex %>"
                                        Text='<%# DataBinder.Eval(Container, "DataItem.FLDNAME").ToString() %>' ></asp:LinkButton>
                                    <asp:Label ID="lblName" runat="server" CommandName="SELECT" CommandArgument="<%# Container.DataItemIndex %>"
                                        Text='<%# DataBinder.Eval(Container, "DataItem.FLDNAME").ToString() %>' visible="false"></asp:Label>
                                     <eluc:ToolTip ID="ucName" runat="server"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'  />      
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterText="Serial No">
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblSerialNoHeader" runat="server" CommandName="Sort" CommandArgument="FLDSERIALNO"
                                        ForeColor="White">Serial Number&nbsp;</asp:Label>
                                    <img id="Img19" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDSERIALNO")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterText="Asset Type">
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="19%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblAssetTypeHeader" runat="server" CommandName="Sort" CommandArgument="FLDASSETTYPEID"
                                        ForeColor="White">Asset&nbsp;</asp:Label>
                                    <img id="Img8" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID ="lblAssetTypeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDASSETTYPEID")%>'></asp:Label>
                                    <asp:Label ID ="lblAssemblyParentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDASSEMBLYPARENTID")%>'></asp:Label>
                                    <asp:Label ID ="lblItemType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDITEMTYPE")%>'></asp:Label>
                                    <asp:Label ID ="lblDisposalDate" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDISPOSALDATE")%>'></asp:Label>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDASSETTYPENAME")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterText="Asset Maker">
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="12.5%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblAssetMakerHeader" runat="server" CommandName="Sort" CommandArgument="FLDMAKER"
                                        ForeColor="White">Maker&nbsp;</asp:Label>
                                    <img id="Img5" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDMAKER")%>
                                </ItemTemplate>
                            </asp:TemplateField>   
                            <asp:TemplateField FooterText="Asset Model">
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="12.5%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblAssetModelHeader" runat="server" CommandName="Sort"
                                        CommandArgument="FLDIDENTIFICATIONNUMBER" ForeColor="White">Model&nbsp;</asp:Label>
                                    <img id="Img18" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDIDENTIFICATIONNUMBER")%>
                                </ItemTemplate>
                            </asp:TemplateField>   
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblZoneHeader" runat="server" CommandName="Sort"
                                        CommandArgument="FLDZONE" ForeColor="White">Zone&nbsp;</asp:Label>
                                    <img id="Img25" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblZoneId" runat="server" Visible="false" CommandName="SELECT" CommandArgument="<%# Container.DataItemIndex %>" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPANYID")%>'></asp:Label>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDZONENAME")%>
                                </ItemTemplate>
                            </asp:TemplateField>                     
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="12%"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="User Mapping" ImageUrl="<%$ PhoenixTheme:images/vendor.png %>"
                                                CommandName="USERMAPPING" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdUserMapping"
                                                ToolTip="Map User"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Software Mapping" ImageUrl="<%$ PhoenixTheme:images/checklist.png %>"
                                                CommandName="SOFTWAREMAPPING" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSoftwareMapping"
                                                ToolTip="Map Software"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Dispose"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage" style="position: relative;z-index: 0">
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
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
