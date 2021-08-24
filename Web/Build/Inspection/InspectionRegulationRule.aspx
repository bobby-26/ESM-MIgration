<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRegulationRule.aspx.cs" Inherits="Inspection_InspectionRegulationRule" %>

<%@ Register TagPrefix="eluc" TagName="Calendar" Src="~/UserControls/UserControlCalenderYear.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Regulation Rule List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
    <style type="text/css">
            .hidden {
                display: none;
            }

            .center {
                background: fixed !important;
            }
            .Label {
            font-weight: bold;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true"></telerik:RadWindowManager>

        <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
        <eluc:Message ID="ucMessage" runat="server" Visible="false"></eluc:Message>
        <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="gvNewRuleTabStrip" Title="Regulation Detail" runat="server" OnTabStripCommand="gvNewRuleTabStrip_TabStripCommand"></eluc:TabStrip>

        <%-- For Popup Relaod --%>
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />

        <table cellspacing="1" width="100%">
            <tr>
                <td style="width: 82px">
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblRegulationTitle" runat="server" CssClass="Label" Text="Regulation"></telerik:RadLabel>
                    <br />
                    <br />
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblIssuedDate" runat="server" CssClass="Label" Text="Issued Date"></telerik:RadLabel>
                    <br />
                    <br />
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblIssuedBy" runat="server" CssClass="Label" Text="Issued By"></telerik:RadLabel>
                </td>
                <td style="width: 200px">
                    <telerik:RadLabel RenderMode="Lightweight" ID="txtRegulationTitle" runat="server" Enabled="true"></telerik:RadLabel>
                    <br />
                    <br />
                    <telerik:RadLabel ID="txtIssuedDate" runat="server"></telerik:RadLabel>
                    <br />
                    <br />
                    <telerik:RadLabel RenderMode="Lightweight" ID="txtIssuedBy" runat="server" Enabled="true"></telerik:RadLabel>
                    <br />
                </td>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblDescription" runat="server" CssClass="Label" Text="Description"></telerik:RadLabel>
                    <br />
                    <br />
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtDescription" runat="server" Enabled="true" Width="50%" Rows="5" TextMode="Multiline"></telerik:RadTextBox>
                </td>
            </tr>
        </table>

        <h3>Rules</h3>

        <eluc:TabStrip ID="RuleTab" runat="server"></eluc:TabStrip>

        <telerik:RadGrid RenderMode="Lightweight" ID="gvNewRule" Width="100%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" ShowFooter="False" Style="margin-bottom: 0px" EnableViewState="true"
            OnNeedDataSource="gvNewRule_NeedDataSource"
            OnItemDataBound="gvNewRule_ItemDataBound"
            OnItemCommand="gvNewRule_ItemCommand" EnableHeaderContextMenu="true" GroupingEnabled="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDRULEID">
                <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                <Columns>

                    <telerik:GridTemplateColumn HeaderText="Rule" AllowSorting="true" SortExpression="">
                        <ItemTemplate>
                            <asp:LinkButton ID="lblRuleName" runat="server" CommandName="SELECT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRULENAME") %>'></asp:LinkButton>
                            <telerik:RadTextBox ID="lblRuleId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRULEID") %>'></telerik:RadTextBox>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox runat="server" ID="txtRuleNameEdit" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRULENAME") %>'></telerik:RadTextBox>
                        </EditItemTemplate>
                        <%--       <FooterTemplate>
                            <telerik:RadTextBox runat="server" ID="txtRuleNameAdd"></telerik:RadTextBox>
                        </FooterTemplate>--%>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Apply" AllowSorting="true" SortExpression="">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblApply" runat="server" Text='<%# Convert.ToBoolean(DataBinder.Eval(Container,"DataItem.FLDAPPLY")) == true ? "Yes" : "No" %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadRadioButtonList runat="server" AutoPostBack="false" ID="chkApplyEdit" Direction="Horizontal">
                                <Items>
                                    <telerik:ButtonListItem Text="Yes" Value="true" />
                                    <telerik:ButtonListItem Text="No" Value="false" />
                                </Items>
                            </telerik:RadRadioButtonList>
                        </EditItemTemplate>
                        <%--           <FooterTemplate>
                            <telerik:RadRadioButtonList runat="server" AutoPostBack="false" ID="chkApplyAdd" Direction="Horizontal">
                                <Items>
                                    <telerik:ButtonListItem Text="Yes" Value="true" Selected="true" />
                                    <telerik:ButtonListItem Text="No" Value="false" />
                                </Items>
                            </telerik:RadRadioButtonList>
                        </FooterTemplate>--%>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="130px">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" ></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" ></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" Visible="false" AlternateText="Edit"
                                CommandName="EDIT" ID="cmdEdit"
                                ToolTip="Edit Rule" Width="20PX" Height="20PX">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Add Attribute"
                                CommandName="ADDATTRIBUTE" ID="cmdAddAttribute"
                                ToolTip="Add Attribute" Width="20PX" Height="20PX">
                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Delete"
                                CommandName="DELETE" ID="cmdDelete"
                                ToolTip="Delete Rule" Width="20PX" Height="20PX">
                                        <span class="icon"><i class="fas fa-trash"></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Save" ID="cmdSave" CommandName="UPDATE" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDRULEID") %>' ToolTip="Save" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Cancel" ID="cmdCancel" CommandName="CANCEL" ToolTip="Cancel" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                            </asp:LinkButton>
                        </EditItemTemplate>
                        <%--        <FooterTemplate>
                            <asp:LinkButton runat="server" AlternateText="Add"
                                CommandName="ADD" ID="cmdRuleAdd"
                                ToolTip="Add Rule" Width="20PX" Height="20PX">
                                        <span class="icon"><i class="fas fa-plus"></i></span>
                            </asp:LinkButton>
                        </FooterTemplate>--%>
                    </telerik:GridTemplateColumn>
                </Columns>
                <NoRecordsTemplate>
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="RadLabel1" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>

        <h3>Attributes</h3>

        <telerik:RadGrid RenderMode="Lightweight" ID="gvNewAttribute" Width="100%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" ShowFooter="False" Style="margin-bottom: 0px" EnableViewState="true"
            OnNeedDataSource="gvNewAttribute_NeedDataSource" GroupingEnabled="true"
            OnItemCommand="gvNewAttribute_ItemCommand"
            OnItemDataBound="gvNewAttribute_ItemDataBound" EnableHeaderContextMenu="true">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>

            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center" DataKeyNames="FLDRULEID">
                <GroupByExpressions>
                    <telerik:GridGroupByExpression>
                        <SelectFields>
                            <telerik:GridGroupByField FieldName="FLDRULENAME" FieldAlias="RULE" SortOrder="Ascending" />
                        </SelectFields>
                        <GroupByFields>
                            <telerik:GridGroupByField FieldName="FLDRULENAME" SortOrder="Ascending" />
                        </GroupByFields>
                    </telerik:GridGroupByExpression>
                </GroupByExpressions>
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
                    <telerik:GridTemplateColumn HeaderText="Sort Order" AllowSorting="true" HeaderStyle-Wrap="false" SortExpression="">
                        <HeaderStyle Width="48px" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblsortorder" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSORTORDER") %>' InputType="Number"></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox ID="txtSortOrderEdit" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSORTORDER") %>' runat="server" InputType="Number"></telerik:RadTextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Attribute" AllowSorting="true" SortExpression="">
                        <HeaderStyle Width="89px" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAttributeId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTRIBUTEID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblFieldName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTRIBUTENAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadDropDownList ID="ddlFieldNameEdit" AutoPostBack="true" OnSelectedIndexChanged="ddlFieldNameEdit_SelectedIndexChanged1" ExpandDirection="Up" runat="server" RenderMode="Lightweight"></telerik:RadDropDownList>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Condition" AllowSorting="true">
                        <HeaderStyle Width="90px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblEarlier" runat="server" Visible="false"></telerik:RadLabel>
                            <telerik:RadLabel ID="lblBefore" runat="server" Visible="false"></telerik:RadLabel>
                            <telerik:RadLabel ID="lblCondition" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONDITION") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadDropDownList ID="ddlConditionEdit" ExpandDirection="Up" runat="server" RenderMode="Lightweight">
                                <Items>
                                    <telerik:DropDownListItem Text="--Select--" Value="0" />
                                    <telerik:DropDownListItem Text="=" Value="1" />
                                    <telerik:DropDownListItem Text=">" Value="2" />
                                    <telerik:DropDownListItem Text=">=" Value="3" />
                                    <telerik:DropDownListItem Text="<" Value="4" />
                                    <telerik:DropDownListItem Text="<=" Value="5" />
                                </Items>
                            </telerik:RadDropDownList>
                            <telerik:RadRadioButtonList ID="chkEarlierLaterEdit" Visible="false" runat="server" AutoPostBack="false" Direction="Horizontal">
                                <Items>
                                    <telerik:ButtonListItem Text="Earlier" Value="<" />
                                    <telerik:ButtonListItem Text="Later" Value=">" />
                                </Items>
                            </telerik:RadRadioButtonList>
                            <telerik:RadRadioButtonList ID="chkBeforeAfterEdit" Visible="false" runat="server" AutoPostBack="false" Direction="Horizontal">
                                <Items>
                                    <telerik:ButtonListItem Text="Before" Value="<" />
                                    <telerik:ButtonListItem Text="After" Value=">" />
                                </Items>
                            </telerik:RadRadioButtonList>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Value" AllowSorting="true">
                        <HeaderStyle Width="190px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblValue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTRIBUTENAME").ToString() ==  "Next Docking" ||
                                DataBinder.Eval(Container,"DataItem.FLDATTRIBUTENAME").ToString() ==  "Year Built"     ?     DataBinder.Eval(Container,"DataItem.FLDVALUE", "{0:dd-MM-yyyy}") : DataBinder.Eval(Container,"DataItem.FLDVALUE")  %>'>
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="lblCode" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICKLISTCODE") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICKLISTNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox ID="txtValueEdit" runat="server" RenderMode="Lightweight" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUE") %>'></telerik:RadTextBox>
                            <span id="spnPickListVesselTypeEdit">
                                <telerik:RadTextBox Visible="false" ID="txtVesselTypeCodeEdit" runat="server" CssClass="input readonlytextbox" MaxLength="20"
                                    ReadOnly="false" Width="60px">
                                </telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtVesselTypeNameEdit" Visible="false" runat="server" CssClass="input readonlytextbox" MaxLength="20"
                                    ReadOnly="false" Width="210px">
                                </telerik:RadTextBox>
                                <asp:ImageButton ID="imgVesselTypeEdit" Visible="false" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                    ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataItem %>" />
                                <telerik:RadTextBox ID="txtVesselTypeIdEdit" runat="server" CssClass="hidden" Width="10px"></telerik:RadTextBox>
                            </span>

                            <span id="spnPickListSurveyTypeEdit">
                                <telerik:RadTextBox Visible="false" ID="txtSurveyCodeEdit" runat="server" CssClass="input readonlytextbox" MaxLength="20"
                                    ReadOnly="false" Width="60px">
                                </telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtSurveyEdit" Visible="false" runat="server" CssClass="input readonlytextbox" MaxLength="20"
                                    ReadOnly="false" Width="210px">
                                </telerik:RadTextBox>
                                <asp:ImageButton ID="imgSurveyEdit" Visible="false" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                    ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataItem %>" />
                                <telerik:RadTextBox ID="txtSurveyIDEdit" runat="server" CssClass="hidden" Width="10px"></telerik:RadTextBox>
                            </span>

                            <span id="spnPickListCertificateEdit">
                                <telerik:RadTextBox Visible="false" ID="txtCerticiateCodeEdit" runat="server" CssClass="input readonlytextbox" MaxLength="20"
                                    ReadOnly="false" Width="60px">
                                </telerik:RadTextBox>
                                <telerik:RadTextBox ID="txCertificateEdit" Visible="false" runat="server" CssClass="input readonlytextbox" MaxLength="20"
                                    ReadOnly="false" Width="210px">
                                </telerik:RadTextBox>
                                <asp:ImageButton ID="imgCertificateEdit" Visible="false" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                    ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataItem %>" />
                                <telerik:RadTextBox ID="txtCertificateIDEdit" runat="server" CssClass="hidden" Width="10px"></telerik:RadTextBox>
                            </span>
                            <telerik:RadDatePicker ID="ddlYearEdit" Visible="false" runat="server" ExpandDirection="Up"></telerik:RadDatePicker>

                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText="Action">
                        <FooterStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="58px"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT"
                                ID="cmdAttributeEdit" ToolTip="Add Attribute" Width="20PX" Height="20PX">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>

                            <asp:LinkButton runat="server" AlternateText="Delete"
                                CommandName="DELETE" ID="cmdAttributeDelete"
                                ToolTip="Delete" Width="20PX" >
                                        <span class="icon"><i class="fas fa-trash"></i></span>
                            </asp:LinkButton>

                            <asp:TextBox runat="server" ID="txtAttributeId" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTRIBUTEID") %>'></asp:TextBox>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Save"
                                CommandName="UPDATE" ID="LinkButton2"
                                ToolTip="Save" Width="20PX" >
                                    <span class="icon"><i class="fas fa-save"></i></span>
                            </asp:LinkButton>

                            <asp:LinkButton runat="server" AlternateText="Cancel"
                                CommandName="CANCEL" ID="LinkButton3"
                                ToolTip="Cancel" Width="20PX" >
                                        <span class="icon"><i class="fas fa-times-circle"></i></span>
                            </asp:LinkButton>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>

                </Columns>

                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>



    </form>
</body>
</html>
