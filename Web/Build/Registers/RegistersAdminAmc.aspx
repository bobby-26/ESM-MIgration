<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersAdminAmc.aspx.cs" Inherits="Registers_RegistersAdminAmc" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Asset AMC</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="assetamc" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmAssetAMC" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlAssetAMC">
        <ContentTemplate>   
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />         
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="AMC" />
                    </div>
                </div>
                <div style="top: 0px; right: 2px; position: absolute">
                    <eluc:TabStrip ID="MenuAsset" runat="server" OnTabStripCommand="Asset_TabStripCommand" 
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <div id="divAssetAMC" style="position: relative; z-index: 2">
                    <table id="tblAssetAMC" width="100%">
                        <tr>
                            <td width= "6.6%">
                                <asp:Literal ID="lblLocation" runat="server" Text="Zone"></asp:Literal> 
                            </td>
                            <td width= "26.6%">
                                <eluc:Zone ID="ddlLocation" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" Width="180px"  AutoPostBack="true"/>
                            </td>
                            <td width= "6.6%">
                                <asp:literal ID="lblAssetName" runat="server" Text="Asset"></asp:literal>
                            </td>
                            <td width= "26.6%">
                                <asp:TextBox ID="txtAssetName" runat="server" CssClass="input" Width="180px"></asp:TextBox>                              
                            </td>
                            <td width= "10.6%">
                                <asp:literal ID="lblNOofDays" runat="server" Text="No of Due Days"></asp:literal>
                            </td>
                            <td width= "22.6%">
                                <eluc:Number ID="txtNoOfDays" runat="server" CssClass="input" MaxLength="99" Width="35px" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuAdminAssetAMC" runat="server" OnTabStripCommand="MenuAdminAssetAMC_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 1">
                    <asp:GridView ID="gvAdminAMC" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvAdminAMC_RowCommand" OnRowDataBound="gvAdminAMC_ItemDataBound"
                        OnRowCancelingEdit="gvAdminAMC_RowCancelingEdit" OnRowDeleting="gvAdminAMC_RowDeleting"
                        OnRowEditing="gvAdminAMC_RowEditing" ShowFooter="true" ShowHeader="true" EnableViewState="false" AllowSorting="true"
                        OnRowUpdating="gvAdminAMC_RowUpdating" OnSorting="gvAdminAMC_Sorting" OnSelectedIndexChanging="gvAdminAMC_SelectedIndexChanging">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="16%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblAssetNameHeader" runat="server">
                                    Asset
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAssetAMCID" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDAMCID")) %>' Visible="false" /> 
                                    <asp:Label ID="lblAssetId" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDASSETID")) %>' Visible="false" /> 
                                    <asp:Label ID="lblAddressId" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDADDRESSID")) %>' Visible="false" /> 
                                    <asp:Label ID="lblAssetNameItem" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDNAME")) %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblLocationId" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCOMPANYID")) %>' Visible="false"></asp:Label>
                                    <asp:LinkButton ID="lblAssetName" runat="server" CommandArgument="<%# Container.DataItemIndex %>" CommandName="SELECT" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDNAME")) %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                   <span id="spnAssetNameEdit">
                                     <asp:TextBox ID="txtAssetNameEdit" runat="server" CssClass="input_mandatory" 
                                       width="150px" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDNAME")) %>'></asp:TextBox>
                                     <asp:ImageButton runat="server" ID="imgAssetNameEdit" Style="cursor: pointer;
                                        vertical-align: top" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" CommandArgument="<%# Container.DataItemIndex %>"/> 
                                     <asp:TextBox ID="txtAssetIdEdit" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDASSETID")) %>' CssClass="input_mandatory" Width="0px"></asp:TextBox> 
                                     <asp:Label ID="lblAssetAMCIDEdit" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDAMCID")) %>' Visible="false"></asp:Label>                                 
                                   </span>
                                </EditItemTemplate> 
                                <FooterTemplate>
                                    <span id="spnAssetNameAdd">
                                     <asp:TextBox ID="txtAssetNameAddSearch" runat="server" CssClass="input_mandatory" width="150px"></asp:TextBox>
                                     <asp:ImageButton runat="server" ID="imgAssetNameAdd" Style="cursor: pointer;
                                        vertical-align: top" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" CommandArgument="<%# Container.DataItemIndex %>"/> 
                                     <asp:TextBox ID="txtAssetIdAdd" runat="server" CssClass="input_mandatory" 
                                         Width="0px"></asp:TextBox>                                  
                                    </span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="25%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblAddressHeader" runat="server">
                                    Address 
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAddressName" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDADDRESSNAME")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <span id="spnAddressEdit">
                                     <asp:TextBox runat="server" ID="txtAddressSupCodeEdit" width="0px"  CssClass="input"></asp:TextBox>                                                                           
                                     <asp:TextBox ID="txtAddressNameEdit" runat="server" CssClass="input_mandatory" Enabled="false"
                                         Width="250px" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDADDRESSNAME")) %>'></asp:TextBox>
                                     <asp:ImageButton runat="server" ID="imgAddressEdit" Style="cursor: pointer;
                                        vertical-align: top" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" CommandArgument="<%# Container.DataItemIndex %>"/>                                   
                                     <asp:TextBox ID="txtAddressCodeEdit" runat="server" width="0px" CssClass="input" 
                                        Enabled="false" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDADDRESSID")) %>' ></asp:TextBox> 
                                   </span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <span id="spnAddressAdd">
                                     <asp:TextBox runat="server" ID="txtAddressSupCodeAdd" CssClass="input" Width="0px"></asp:TextBox>
                                     <asp:TextBox ID="txtAddressNameAdd" runat="server" CssClass="input_mandatory" Enabled="false" Width="250px"></asp:TextBox> 
                                     <asp:ImageButton runat="server" ID="imgAddressAdd" Style="cursor: pointer;
                                        vertical-align: top" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" CommandArgument="<%# Container.DataItemIndex %>"/>                                   
                                     <asp:TextBox ID="txtAddressCodeAdd" runat="server" CssClass="input" Width="0px"
                                        Enabled="false"></asp:TextBox>     
                                    </span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="8%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>                               
                                 <HeaderTemplate>
                                    <asp:Label ID="lblAddressHeader" runat="server">
                                    PO Reference 
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPOReference" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDPOREFERENCE")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtPoreferenceEdit" runat="server" CssClass="input" MaxLength="80" Width="120px" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDPOREFERENCE")) %>'></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtPoreferenceAdd" runat="server" CssClass="input" MaxLength="80" Width="120px"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle> 
                                <FooterStyle HorizontalAlign="Right"/>                              
                                 <HeaderTemplate>
                                        <asp:label ID="lblDurationHeader" runat="server">Duration(In Months) </asp:label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDuration" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDDURATION")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="ucDurationEdit" runat="server" CssClass="input" MaxLength="3" Width="35px" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDDURATION")) %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number ID="ucDurationAdd" runat="server" CssClass="input" MaxLength="3" Width="35px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="8%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblLastdoneHeader" runat="server" CommandName="Sort" CommandArgument="FLDLASTDONE"
                                        ForeColor="White">Last done</asp:Label>
                                    <img id="FLDLASTDONE" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblLastDone" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDLASTDONE")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:UserControlDate ID="ucLastDoneEdit" runat="server" DatePicker="true" CssClass="input" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDLASTDONE")) %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:UserControlDate ID="ucLastdoneAdd" runat="server" DatePicker="true" CssClass="input" />
                                </FooterTemplate>
                            </asp:TemplateField> 
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="8%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblNextDueHeader" runat="server" CommandName="SORT" CommandArgument="FLDNEXTDUE"
                                        ForeColor="White">Next Due</asp:Label>
                                    <img id="FLDNEXTDUE" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblNextDue" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDNEXTDUE")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblNextDueEdit" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDNEXTDUE")) %>'></asp:Label>
                                    <%--<eluc:UserControlDate ID="ucNextdueEdit" runat="server" DatePicker="true" CssClass="input" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDNEXTDUE")) %>' />--%>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <%--<eluc:UserControlDate ID="ucNextdueAdd" runat="server" DatePicker="true" CssClass="input" />--%>
                                </FooterTemplate>
                            </asp:TemplateField> 
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="8%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDoneDateHeader" runat="server" CommandName="SORT" CommandArgument="FLDNEXTDUE"
                                        ForeColor="White">Done Date</asp:Label>
                                    <img id="FLDNEXTDUE" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDoneDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDONEDATE")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblDoneDateEdit" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDONEDATE")) %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>                          
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="5%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActiveHeader" runat="server" CommandName="Sort" CommandArgument="FLDSTATUS"
                                        ForeColor="White">Active</asp:Label>
                                    <img id="FLDSTATUS" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblActiveItem" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblActive" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME")) %>'></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                </FooterTemplate>
                            </asp:TemplateField>    
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="7%"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Done Date" ImageUrl="<%$ PhoenixTheme:images/31.png %>"
                                        CommandName="Done Date" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDoneDate"
                                        ToolTip="Done Date"></asp:ImageButton>
                                    <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Save" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="ADD" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage" style="position: relative; z-index:0">
                    <table width="100%" border="0" class="datagrid_pagestyle">
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
