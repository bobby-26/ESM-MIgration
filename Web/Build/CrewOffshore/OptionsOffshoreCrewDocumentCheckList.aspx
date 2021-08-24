<%@ Page Language="C#" AutoEventWireup="True" CodeFile="OptionsOffshoreCrewDocumentCheckList.aspx.cs"
    Inherits="OptionsOffshoreCrewDocumentCheckList" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Document Checklist</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersFlag" runat="server" submitdisabledcontrols="true">
        <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlFlagEntry">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <div class="subHeader" style="position: relative">
                        <div id="divHeading">
                            <eluc:Title runat="server" ShowMenu="false" ID="ucTitle" Text="Crew Document Checklist"></eluc:Title>
                        </div>
                    </div>

                    <div style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="MenuChecklist" runat="server" OnTabStripCommand="Checklist_TabStripCommand" TabStrip="false"></eluc:TabStrip>
                        <asp:Button runat="server" ID="cmdHiddenSubmit" Visible="false" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                    <table runat="server" id="tblPersonalMaster" width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblFirstName" runat="server" Text="Name"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>


                            <td runat="server" id="tdempno">
                                <asp:Literal ID="lblEmployeeNo" runat="server" Text="File Number"></asp:Literal>
                            </td>
                            <td runat="server" id="tdempnum">
                                <asp:TextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblvessel" runat="server" Text="Vessel"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtVessel" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <asp:TextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                            
                        </tr>
                        <tr>
                            <td colspan="6">
                                <hr />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div>
                        <font color="blue"><b>
                            <asp:Label ID="lblNote" runat="server" Style="font-size: 13px;" Text="*Note:"></asp:Label>
                        </b>
                            <br />
                            &nbsp&nbsp&nbsp
                            <asp:Label ID="lblNoteofpage" runat="server" Style="font-size: 13px;"
                                Text=" Kindly go through the list and confirm for each document by updating 
                                YES / NO in the ‘Holding Original Y/N’ Column."></asp:Label>
                            <br />
                        </font>
                    </div>
                    <br />
                    <div id="divGrid" style="position: relative; z-index: 0">
                        <asp:GridView ID="gvDocumentChecklist" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            OnRowCreated="gvDocumentChecklist_RowCreated" Width="100%" CellPadding="3" OnRowCommand="gvDocumentChecklist_RowCommand"
                            OnRowDataBound="gvDocumentChecklist_ItemDataBound" OnRowCancelingEdit="gvDocumentChecklist_RowCancelingEdit"
                            OnRowDeleting="gvDocumentChecklist_RowDeleting" OnRowUpdating="gvDocumentChecklist_RowUpdating" OnRowEditing="gvDocumentChecklist_RowEditing"
                            ShowHeader="true" EnableViewState="false" AllowSorting="true"
                            OnSorting="gvDocumentChecklist_Sorting">
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />

                            <Columns>
                                <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                                <asp:TemplateField HeaderText="Abbreviation">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <%-- <asp:Label ID="lblCodeHeader" Visible="true" runat="server">
                                            <asp:ImageButton runat="server" ID="cmdAbbreviation" OnClick="cmdSearch_Click" CommandName="FLDABBREVIATION"
                                                ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" CommandArgument="1" />
                                        </asp:Label>
                                        <asp:LinkButton ID="lnkAbbreviationHeader" runat="server" CommandName="Sort" CommandArgument="FLDABBREVIATION"
                                            ForeColor="White">Code&nbsp;</asp:LinkButton>
                                        <img id="FLDABBREVIATION" runat="server" visible="false" />--%>
                                        <asp:Label ID="lblDocumentCategoryHead" runat="server" Text="Document Category"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDocumentcategoryIdItem" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTCATEGORY") %>'></asp:Label>
                                        <asp:Label ID="lblDocumentchecklistiditem" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTCHECKLISTID") %>'></asp:Label>
                                        <asp:Label ID="lblDocumentcategoryNameItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></asp:Label>
                                        <asp:Label ID="lblDocumentTypeitemId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></asp:Label>
                                        <asp:Label ID="lblDocumentchecklistidedit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTCHECKLISTID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblDocumentTypeId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></asp:Label>
                                        <asp:Label ID="lblDocumentchecklistidedit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTCHECKLISTID") %>'></asp:Label>
                                        <asp:Label ID="lblDocumentcategoryIdEdit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTCATEGORY") %>'></asp:Label>
                                        <asp:Label ID="lblDocumentcategoryNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></asp:Label>
                                        <%--<asp:Label ID="lblAbbreviationEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDABBREVIATION") %>'></asp:Label>--%>
                                    </EditItemTemplate>

                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Flag Name">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <%--   <asp:LinkButton ID="lblHeader" runat="server" CommandName="Sort" CommandArgument="FLDFLAGNAME"
                                            ForeColor="White">Name&nbsp;</asp:LinkButton>
                                        <img id="FLDFLAGNAME" runat="server" visible="false" />--%>
                                        <asp:Label ID="lblRequiredDocumentNameHead" runat="server" Text="Required Document"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequiredDocumentIdItem" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTID") %>'></asp:Label>
                                        <asp:Label ID="lblRequiredDocumentNameItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'></asp:Label>
                                        <%--<asp:Label ID="lblFlagId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLAGID") %>'></asp:Label>
                                        <asp:LinkButton ID="lnkFlagName" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataItemIndex %>'
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLAGNAME") %>'></asp:LinkButton>--%>
                                    </ItemTemplate>
                                    <EditItemTemplate>

                                        <%--<asp:Label ID="lblFlagIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLAGID") %>'></asp:Label>
                                        <eluc:Country runat="server" ID="ucCountryEdit" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                            CountryList='<%# PhoenixRegistersCountry.ListCountry(1) %>' SelectedCountry='<%# DataBinder.Eval(Container, "DataItem.FLDCOUNTRYCODE") %>' />--%>
                                        <asp:Label ID="lblRequiredDocumentNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'></asp:Label>
                                    </EditItemTemplate>

                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblavailableDocumentHead" runat="server" Text="Available Document"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblAvailableDocumentIdItem" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVAILABLEDOCUMENTID") %>'></asp:Label>
                                        <asp:Label ID="lblAvailableDocumentNameItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVAILABLEDOCUMENTNAME") %>'></asp:Label>
                                        <%--<asp:Label ID="lblReportCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTCODE") %>'></asp:Label>--%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <%--<eluc:Hard runat="server" ID="ucReportCodeEdit" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                            HardList='<%# PhoenixRegistersHard.ListHard(1, 122) %>' SelectedHard='<%# DataBinder.Eval(Container, "DataItem.FLDREPORTCODEID") %>' />--%>
                                        <asp:Label ID="lblAvailableDocumentIdEdit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVAILABLEDOCUMENTID") %>'></asp:Label>
                                        <asp:Label ID="lblAvailableDocumentNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVAILABLEDOCUMENTNAME") %>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Holding Original Y/N">
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblHoldingOriginalYNHead" runat="server" Text="Holding Original Y/N"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate >
                                        <asp:Label ID="lblHoldingYNItem" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHOLDINGYESNO") %>'></asp:Label>
                                        <asp:CheckBox ID="ckbYesOrNo" runat="server" AutoPostBack="true" Checked='<%# (DataBinder.Eval(Container, "DataItem.FLDHOLDINGYESNO")).ToString()=="1" ?true:false %>'  />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblHoldingYNidEdit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHOLDINGYN") %>'></asp:Label>
                                        <asp:DropDownList Width="100%" ID="ddlHoldingynEdit" runat="server" CssClass="dropdown_mandatory">
                                            <asp:ListItem Text="-Select-" Value=""></asp:ListItem>
                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                        <%--<asp:CheckBox ID="chkMedicalRequiresYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDMEDICALREQUIRES").ToString().Equals("1"))?true:false %>'></asp:CheckBox>--%>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Where is original">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="140px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblOriginalremark" runat="server" Text="If 'No'.Where is the Original"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="lblifnoremarksItem" runat="server" TextMode="MultiLine" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></asp:TextBox>
                                        <%--<asp:Label ID="lblifnoremarksItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS").ToString().Length > 20?HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString()).ToString().Substring(0, 20)+ "..." : HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString()) %>'></asp:Label>--%>
                                        <eluc:Tooltip ID="ucToolTipremark" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDREMARKS") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <%--<asp:CheckBox ID="chkFlagSibYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDFLAGSIBYN").ToString().Equals("1"))?true:false %>'></asp:CheckBox>--%>
                                        <asp:TextBox ID="lblifnoremarksEdit" runat="server" TextMode="MultiLine" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                        </asp:Label>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                            CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>
                                        <img alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                            CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                            ToolTip="Save"></asp:ImageButton>
                                        <img alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                            ToolTip="Cancel"></asp:ImageButton>
                                    </EditItemTemplate>

                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <%--<div id="divPage" style="position: relative;">
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
                           
                        </table>
                    </div>--%>
                    <div>
                        <input type="hidden" id="isouterpage" name="isouterpage" />
                        <eluc:Status runat="server" ID="ucStatus" />
                    </div>


                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
