<%@ Page Language="C#" AutoEventWireup="True" CodeFile="OptionsOffshoreCrewDocumentCheckList.aspx.cs"
    Inherits="OptionsOffshoreCrewDocumentCheckList" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Document Checklist</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmRegistersFlag" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">

            <eluc:TabStrip ID="MenuChecklist" runat="server" Title="Crew Document Checklist" OnTabStripCommand="Checklist_TabStripCommand" TabStrip="false"></eluc:TabStrip>
            <asp:Button runat="server" ID="cmdHiddenSubmit" Visible="false" OnClick="cmdHiddenSubmit_Click" />

            <table runat="server" id="tblPersonalMaster" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>


                    <td runat="server" id="tdempno">
                        <telerik:RadLabel ID="lblEmployeeNo" runat="server" Text="File Number"></telerik:RadLabel>
                    </td>
                    <td runat="server" id="tdempnum">
                        <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblvessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtVessel" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
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
                    <telerik:RadLabel ID="lblNote" runat="server" Style="font-size: 13px;" Text="*Note:"></telerik:RadLabel>
                </b>
                    <br />
                    &nbsp&nbsp&nbsp
                            <telerik:RadLabel ID="lblNoteofpage" runat="server" Style="font-size: 13px;"
                                Text=" Kindly go through the list and confirm for each document by updating 
                                YES / NO in the ‘Holding Original Y/N’ Column.">
                            </telerik:RadLabel>
                    <br />
                </font>
            </div>
            <br />
            <div id="divGrid" style="position: relative; z-index: 0">
                <%--  <asp:GridView ID="gvDocumentChecklist" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    OnRowCreated="gvDocumentChecklist_RowCreated" Width="100%" CellPadding="3" OnRowCommand="gvDocumentChecklist_RowCommand"
                    OnRowDataBound="gvDocumentChecklist_ItemDataBound" OnRowCancelingEdit="gvDocumentChecklist_RowCancelingEdit"
                    OnRowDeleting="gvDocumentChecklist_RowDeleting" OnRowUpdating="gvDocumentChecklist_RowUpdating" OnRowEditing="gvDocumentChecklist_RowEditing"
                    ShowHeader="true" EnableViewState="false" AllowSorting="true"
                    OnSorting="gvDocumentChecklist_Sorting">
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
             
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvDocumentChecklist" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvDocumentChecklist_NeedDataSource"
                        OnItemCommand="gvDocumentChecklist_ItemCommand"
                        OnSortCommand="gvDocumentChecklist_SortCommand"
                        OnItemDataBound="gvDocumentChecklist_ItemDataBound"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                  
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed">
                            <NoRecordsTemplate>
                                <table width="100%" border="0">
                                    <tr>
                                        <td align="center">
                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </NoRecordsTemplate>
                            <HeaderStyle Width="102px" />
                            <Columns>
                             
                                <telerik:GridTemplateColumn HeaderText="Abbreviation">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                  
                                        <telerik:RadLabel ID="lblDocumentCategoryHead" runat="server" Text="Document Category"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDocumentcategoryIdItem" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTCATEGORY") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblDocumentchecklistiditem" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTCHECKLISTID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblDocumentcategoryNameItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblDocumentTypeitemId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblDocumentchecklistidedit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTCHECKLISTID") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblDocumentTypeId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblDocumentchecklistidedit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTCHECKLISTID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblDocumentcategoryIdEdit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTCATEGORY") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblDocumentcategoryNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                                        <%--<telerik:RadLabel ID="lblAbbreviationEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDABBREVIATION") %>'></telerik:RadLabel>--%>
                                    </EditItemTemplate>

                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Flag Name">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <%--   <asp:LinkButton ID="lblHeader" runat="server" CommandName="Sort" CommandArgument="FLDFLAGNAME"
                                            ForeColor="White">Name&nbsp;</asp:LinkButton>
                                        <img id="FLDFLAGNAME" runat="server" visible="false" />--%>
                                        <telerik:RadLabel ID="lblRequiredDocumentNameHead" runat="server" Text="Required Document"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblRequiredDocumentIdItem" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblRequiredDocumentNameItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'></telerik:RadLabel>
                                        <%--<telerik:RadLabel ID="lblFlagId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLAGID") %>'></telerik:RadLabel>
                                        <asp:LinkButton ID="lnkFlagName" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataItemIndex %>'
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLAGNAME") %>'></asp:LinkButton>--%>
                                    </ItemTemplate>
                                    <EditItemTemplate>

                                        <%--<telerik:RadLabel ID="lblFlagIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLAGID") %>'></telerik:RadLabel>
                                        <eluc:Country runat="server" ID="ucCountryEdit" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                            CountryList='<%# PhoenixRegistersCountry.ListCountry(1) %>' SelectedCountry='<%# DataBinder.Eval(Container, "DataItem.FLDCOUNTRYCODE") %>' />--%>
                                        <telerik:RadLabel ID="lblRequiredDocumentNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'></telerik:RadLabel>
                                    </EditItemTemplate>

                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblavailableDocumentHead" runat="server" Text="Available Document"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblAvailableDocumentIdItem" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVAILABLEDOCUMENTID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblAvailableDocumentNameItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVAILABLEDOCUMENTNAME") %>'></telerik:RadLabel>
                                        <%--<telerik:RadLabel ID="lblReportCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTCODE") %>'></telerik:RadLabel>--%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <%--<eluc:Hard runat="server" ID="ucReportCodeEdit" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                            HardList='<%# PhoenixRegistersHard.ListHard(1, 122) %>' SelectedHard='<%# DataBinder.Eval(Container, "DataItem.FLDREPORTCODEID") %>' />--%>
                                        <telerik:RadLabel ID="lblAvailableDocumentIdEdit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVAILABLEDOCUMENTID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblAvailableDocumentNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVAILABLEDOCUMENTNAME") %>'></telerik:RadLabel>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Holding Original Y/N">
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblHoldingOriginalYNHead" runat="server" Text="Holding Original Y/N"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblHoldingYNItem" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHOLDINGYESNO") %>'></telerik:RadLabel>
                                        <asp:CheckBox ID="ckbYesOrNo" runat="server" AutoPostBack="true" Checked='<%# (DataBinder.Eval(Container, "DataItem.FLDHOLDINGYESNO")).ToString()=="1" ?true:false %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblHoldingYNidEdit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHOLDINGYN") %>'></telerik:RadLabel>
                                        <asp:DropDownList Width="100%" ID="ddlHoldingynEdit" runat="server" CssClass="dropdown_mandatory">
                                            <asp:ListItem Text="-Select-" Value=""></asp:ListItem>
                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                        <%--<asp:CheckBox ID="chkMedicalRequiresYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDMEDICALREQUIRES").ToString().Equals("1"))?true:false %>'></asp:CheckBox>--%>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Where is original">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="140px"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblOriginalremark" runat="server" Text="If 'No'.Where is the Original"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadTextBox ID="lblifnoremarksItem" runat="server" TextMode="MultiLine" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadTextBox>
                                        <%--<telerik:RadLabel ID="lblifnoremarksItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS").ToString().Length > 20?HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString()).ToString().Substring(0, 20)+ "..." : HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString()) %>'></telerik:RadLabel>--%>
                                        <eluc:Tooltip ID="ucToolTipremark" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDREMARKS") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <%--<asp:CheckBox ID="chkFlagSibYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDFLAGSIBYN").ToString().Equals("1"))?true:false %>'></asp:CheckBox>--%>
                                        <telerik:RadTextBox ID="lblifnoremarksEdit" runat="server" TextMode="MultiLine" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadTextBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn Visible="false">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblActionHeader" runat="server">
                                            Action
                                        </telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                            CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>
                                        <img alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                            CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                            ToolTip="Save"></asp:ImageButton>
                                        <img alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                            ToolTip="Cancel"></asp:ImageButton>
                                    </EditItemTemplate>

                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="false" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
            </div>

            <div>
                <input type="hidden" id="isouterpage" name="isouterpage" />
                <eluc:Status runat="server" ID="ucStatus" />
            </div>


        </div>

    </form>
</body>
</html>
