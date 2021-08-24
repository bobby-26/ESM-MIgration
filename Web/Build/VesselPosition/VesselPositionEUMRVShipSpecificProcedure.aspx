<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionEUMRVShipSpecificProcedure.aspx.cs" Inherits="VesselPositionEUMRVShipSpecificProcedure" %>

<%@ Import Namespace="SouthNests.Phoenix.VesselPosition" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Location</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="VesselDirectionlink" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmVPRSLocation" runat="server">
        <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlVPRSLocation">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <div class="subHeader" style="position: relative">
                        <div id="divHeading">
                            <eluc:Title runat="server" ID="ucTitle" Text="Ship Specific Procedure"></eluc:Title>
                        </div>
                        <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                            <eluc:TabStrip ID="MenuProcedureDetailList" runat="server" OnTabStripCommand="MenuProcedureDetailList_TabStripCommand"></eluc:TabStrip>
                        </div>
                    </div>
                    <div>
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Label ID="lblvessel" Text="Vessel" runat="server"></asp:Label></td>
                                <td>
                                    <eluc:Vessel ID="UcVessel" runat="server" CssClass="input_mandatory" VesselsOnly="true"
                                        AppendDataBoundItems="true" AutoPostBack="true" OnTextChangedEvent="ucVessel_Changed" />
                                </td>
                                <td>
                                    <asp:Label ID="lblcopytovessel" Text="Copy to Vessel" runat="server" Visible="false"></asp:Label></td>
                                <td>
                                    <eluc:Vessel ID="UcToVessel" runat="server" CssClass="input" VesselsOnly="true"
                                        AppendDataBoundItems="true" AutoPostBack="true" Visible="false" />
                                </td>
                            </tr>

                        </table>
                        <hr />
                        <table width="100%">
                            <tr>

                                <td>
                                    <asp:Label ID="lblCode" Text="Table" runat="server"></asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtCode" runat="server" CssClass="input"></asp:TextBox></td>
                                <td>
                                    <asp:Label ID="lblprocedurefilter" Text="Procedure" runat="server"></asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtprocedurefilter" runat="server" CssClass="input"></asp:TextBox></td>

                            </tr>
                        </table>

                    </div>
                    <div class="navSelect" style="position: relative; width: 15px">
                        <eluc:TabStrip ID="MenuLocation" runat="server" OnTabStripCommand="Location_TabStripCommand"></eluc:TabStrip>
                    </div>

                    <div id="divGrid" style="position: relative; z-index: 0">
                        <asp:GridView ID="gvProcedure" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnRowCommand="gvProcedure_RowCommand" OnRowDataBound="gvProcedure_ItemDataBound"
                            OnRowCreated="gvProcedure_RowCreated" OnRowCancelingEdit="gvProcedure_RowCancelingEdit"
                            OnRowDeleting="gvProcedure_RowDeleting" AllowSorting="true" OnRowEditing="gvProcedure_RowEditing"
                            OnRowUpdating="gvProcedure_RowUpdating" ShowFooter="true"
                            ShowHeader="true" EnableViewState="false" OnSorting="gvProcedure_Sorting">

                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <Columns>
                                <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />

                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lblCodeHeader" runat="server" CommandName="Sort" CommandArgument="FLDCODE"
                                            ForeColor="White">Table</asp:LinkButton>
                                        <img id="FLDCODE" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblProcedureCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtProcedureCodeEdit" Width="99%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>' runat="server" CssClass="gridinput_mandatory" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtProcedureCodeAdd" runat="server" Width="99%" CssClass="gridinput_mandatory" />
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lblProcedureHeader" runat="server" CommandName="Sort" CommandArgument="FLDPROCEDURE"
                                            ForeColor="White">Procedure(Guidance)</asp:LinkButton>
                                        <img id="FLDPROCEDURE" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lblProcedure" runat="server" CommandName="NAV" CommandArgument='<%# Container.DataItemIndex %>'
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCEDUREGUIDANCE") %>'></asp:LinkButton>
                                        <asp:Label ID="lblProcedureIdadd" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEUMRVPROCEDUERID") %>'></asp:Label>
                                        <asp:Label ID="lblProcedureDetailID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERSIONDETAILID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <b>Procedure</b>
                                                    <asp:TextBox ID="txtProcedureEdit" Width="85%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCEDURE") %>' runat="server" CssClass="gridinput_mandatory" />
                                                    <asp:Label ID="lblProcedureId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEUMRVPROCEDUERID") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <b>Guidance&nbsp;&nbsp;&nbsp;</b><asp:TextBox ID="txtGuidanceEdit" Width="85%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGUIDANCE") %>' runat="server" CssClass="input" />
                                                </td>
                                            </tr>
                                        </table>


                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <b>Procedure</b>
                                                    <asp:TextBox ID="txtProcedureAdd" runat="server" Width="85%" CssClass="gridinput_mandatory" /><br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <b>Guidance&nbsp;&nbsp;</b>
                                                    <asp:TextBox ID="txtGuidanceAdd" Width="85%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGUIDANCE") %>' runat="server" CssClass="gridinput" />
                                                </td>
                                            </tr>
                                        </table>
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField Visible="false">
                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkVersionHeader" runat="server" CommandName="Sort" CommandArgument="FLDVERSION"
                                            ForeColor="White">Version</asp:LinkButton>
                                        <img id="FLDVERSION" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>

                                        <asp:Label ID="lblLocation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERSION") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblVersionIdedit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEUMRVPROCEDUERID") %>'></asp:Label>
                                        <asp:Label ID="lblVersionedit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERSION") %>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblActionHeader" runat="server">
                                        Action
                                        </asp:Label>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                            CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>
                                        <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                            ToolTip="Delete"></asp:ImageButton>
                                        <img id="Img4" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" AlternateText="View" ImageUrl="<%$ PhoenixTheme:images/view-task.png %>"
                                            CommandName="VIEW" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdView"
                                            ToolTip="View"></asp:ImageButton>
                                        <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                            CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                            ToolTip="Save"></asp:ImageButton>
                                        <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                            ToolTip="Cancel"></asp:ImageButton>
                                    </EditItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                    <FooterTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                            CommandName="Add" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                            ToolTip="Add New"></asp:ImageButton>
                                    </FooterTemplate>
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
                                    <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
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
                            <eluc:Status runat="server" ID="ucStatus" />
                        </table>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
