<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsOfficePortageBillPostingBreakUp.aspx.cs"
    Inherits="AccountsOfficePortageBillPostingBreakUp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register Src="~/UserControls/UserControlVessel.ascx" TagName="Vessel" TagPrefix="eluc" %>
<%@ Register Src="~/UserControls/UserControlStatus.ascx" TagName="Status" TagPrefix="eluc" %>
<%@ Register Src="../UserControls/UserControlRank.ascx" TagName="Rank" TagPrefix="eluc" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew BOW</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript">
            function RegisterOnChangeMethod(ctrlid) {
                if ("\v" == "v") {
                    $addHandler($get(ctrlid), "propertychange", OnTextValueChange);
                }
                else {
                    // only support for FF 3.0.10 or later
                    $get(ctrlid).addEventListener("DOMAttrModified", OnTextValueChange, false);
                }
            }
            function OnTextValueChange() {
                document.getElementById("cmdHiddenSubmit").click();
            }

        </script>
        <style type="text/css">
            .RadGrid .rgHeader, .RadGrid th.rgResizeCol, .RadGrid .rgRow td, .RadGrid .rgAltRow td {
                padding-left: 2px !important;
                padding-right: 2px !important;
                align-items: center !important;
            }
        </style>
        <style type="text/css">
            .hidden {
                display: none;
            }
            .center {
                background: fixed !important;
            }
        </style>

    </telerik:RadCodeBlock>
</head>
<body>

    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmCrewList" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server">
        </telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="90%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click"
                CssClass="hidden" />
            <eluc:TabStrip ID="MenuPB" runat="server" OnTabStripCommand="MenuPB_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuPB1" runat="server" OnTabStripCommand="MenuPB1_TabStripCommand"></eluc:TabStrip>
            <table width="100%">
                <%-- <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblFilter" runat="server" Text="Filter"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>--%>
                <tr>

                    <td colspan="2">
                        <telerik:RadTextBox ID="txtvesselperiod" runat="server" ReadOnly="true" Width="99%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblfileno" runat="server" Text="File No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtfileno" runat="server"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblempname" runat="server" Text="Employee"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtempname" runat="server" Width="200px"></telerik:RadTextBox>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRank" Text="Rank" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank ID="ddlRank" runat="server" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbldescription" Text="Description" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtdescripton" runat="server" Width="300px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbldocument" runat="server" Text="Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddldocument" runat="server" DataTextField="FLDDESCRIPTION"
                            DataValueField="FLDTYPE" AppendDataBoundItems="true">
                            <Items>
                                <telerik:DropDownListItem Text="--Select--" Value="" />
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblbudget" Text="Budget Code" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListMainBudget">
                            <telerik:RadTextBox ID="txtBudgetCode" runat="server" Width="60px" CssClass="input_mandatory"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBudgetName" runat="server" Width="180px" CssClass="input_mandatory"></telerik:RadTextBox>
                            <asp:ImageButton ID="btnShowBudget" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtBudgetId" runat="server" Width="0px" CssClass="input hidden"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBudgetgroupId" runat="server" Width="0px" CssClass="input hidden"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblownerbudgetcode" runat="server">Owners Budget Code</telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListOwnerBudget">
                            <telerik:RadTextBox ID="txtAccountCode1" runat="server" MaxLength="20" CssClass="input_mandatory"
                                Width="246">
                            </telerik:RadTextBox>
                            <asp:ImageButton ID="imgShowAccount1" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtOwnerBudgetNameEdit" runat="server" Width="0px" CssClass="input hidden"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtownerbudgetMapidEdit" runat="server" Width="0px" CssClass="input hidden" Text=""></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtOwnerBudgetgroup" runat="server" Width="0px" CssClass="input hidden"></telerik:RadTextBox>
                        </span>

                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuPBExcel" runat="server" OnTabStripCommand="MenuPBExcel_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvPB" Height="73%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvPB_ItemCommand" OnItemDataBound="gvPB_ItemDataBound" EnableHeaderContextMenu="true"
                ShowFooter="false" ShowHeader="true" EnableViewState="true" OnNeedDataSource="gvPB_NeedDataSource">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                    AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                    <GroupByExpressions>
                        <telerik:GridGroupByExpression>
                            <SelectFields>
                                <telerik:GridGroupByField FieldName="FLDGROUPNAME" FieldAlias="Details" />
                            </SelectFields>
                            <GroupByFields>
                                <telerik:GridGroupByField FieldName="FLDEMPLOYEEID" />
                            </GroupByFields>
                        </telerik:GridGroupByExpression>
                    </GroupByExpressions>
                    <%--<HeaderStyle Width="102px" />--%>
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn Visible="false" UniqueName="Data" DataField="FLDEMPLOYEEID">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblGroup" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDEMPLOYEEID"]%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="Listcheckbox">
                            <HeaderStyle Width="20px" HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadCheckBox ID="chkSelectAll" runat="server" EnableViewState="true" AutoPostBack="true" OnPreRender="SelectAll_CheckedChanged" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="chkSelect" runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>

                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Component">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDCOMPONENTNAME"].ToString() %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Description">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDDESCRIPTION"].ToString() %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Account Code">
                            <HeaderStyle Width="16%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDACCOUNTCODE"].ToString() + " - " + ((DataRowView)Container.DataItem)["FLDACCOUNTDESCRIPTION"].ToString()%>
                                <telerik:RadLabel ID="lbldtKey" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDDTKEY"].ToString() %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblvesselid" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDVESSELID"].ToString() %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDVESSELID"].ToString() %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmpId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDEMPLOYEEID"].ToString() %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblPBId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDPORTAGEBILLID"].ToString() %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDtKey" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDDTKEY"].ToString() %>'></telerik:RadLabel>
                                <span id="spnPickListCompanyAccountEdit">
                                    <telerik:RadTextBox ID="txtAccountCode" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'
                                        MaxLength="20" Width="80px" Enabled="false">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtAccountDescription" runat="server" CssClass="input_mandatory"
                                        Enabled="False" MaxLength="50" Width="10px">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowAccountEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." />
                                    <telerik:RadTextBox ID="txtAccountId" runat="server" MaxLength="20" Width="10px"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'>
                                    </telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Budget Code">
                            <HeaderStyle Width="19%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDBUDGETCODE"].ToString()%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListBudgetEdit">
                                    <telerik:RadTextBox ID="txtBudgetCodeEdit" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE") %>'
                                        MaxLength="20" CssClass="input_mandatory" Width="50%" Enabled="false">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetNameEdit" runat="server" Width="0px" CssClass="input hidden"
                                        Enabled="False">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowBudgetEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." />
                                    <telerik:RadTextBox ID="txtBudgetIdEdit" runat="server" Width="0px" CssClass="input hidden"
                                        Text='<%#DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'>
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetgroupIdEdit" runat="server" Width="0px" CssClass="input hidden"></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Owner Budget Code">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDOWNERBUDGETGROUP"].ToString()%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListOwnerBudgetEdit">
                                    <telerik:RadTextBox ID="txtAccountCode1Edit" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETGROUP") %>'
                                        MaxLength="20" CssClass="input_mandatory" Width="50%">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="imgShowAccount1Edit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." />
                                    <telerik:RadTextBox ID="TextBox1Edit" runat="server" Width="0px" CssClass="input hidden"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtownerbudgetMapidEdit" runat="server" Width="0px" CssClass="input hidden" Text='<%#DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETCODEMAPID") %>'></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtOwnerBudgetgroupEdit" runat="server" Width="0px" CssClass="input hidden"></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDAMOUNT"].ToString()%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["DOCUMENTTYPE"].ToString()%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Split" CommandName="SPLIT" ID="cmdSplit" ToolTip="Split">
                                    <span class="icon"><i class="fas fa-multiplepo"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Save" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>

                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <%--
                           <asp:ImageButton runat="server" AlternateText="Split" ImageUrl="<%$ PhoenixTheme:images/ %>"
                                CommandName="" CommandArgument="<%# Container.DataItemIndex %>" ID=""
                                ToolTip=""></asp:ImageButton>
                        </ItemTemplate>
            --%>

            <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
