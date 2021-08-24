<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOfferLetter.aspx.cs"
    Inherits="CrewOfferLetter" ValidateRequest="false" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CrewComponents" Src="~/UserControls/UserControlContractCrew.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MCUser" Src="~/UserControls/UserControlMultiColumnUser.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Appraisal Activity</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmCrewOfferLetter" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="frmCrewOfferLetter" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager runat="server" ID="RadSkinManager1"></telerik:RadSkinManager>
        <eluc:TabStrip ID="CrewOfferLetterTabs" runat="server" OnTabStripCommand="CrewOfferLetterTabs_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <%--OnTabStripCommand="CrewOfferLetterTabs_TabStripCommand" OnTabStripCommand="CrewOfferLetter_TabStripCommand"--%>
        <%--<eluc:TabStrip ID="CrewOfferLetter" runat="server" Title="Appraisal Form"></eluc:TabStrip>--%>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="85%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenPick" runat="server" OnClick="cmdHiddenPick_Click" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <div id="divPrimarySection" runat="server">
                <table width="100%" cellspacing="0" cellpadding="1" border="1" style="width: 99%;" rules="all">
                    <tr>
                        <td style="border-right: hidden">
                            <telerik:RadLabel ID="lblName" runat="server" Text="Name of the seafarer / File No"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtEmployeeName" runat="server" ReadOnly="true" Enabled="false" CssClass="readonlytextbox" Width="180px"></telerik:RadTextBox>/
                            <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true" Enabled="false"></telerik:RadTextBox>
                        </td>

                        <td style="border-right: hidden">
                            <telerik:RadLabel ID="lblRank" runat="server" Text="Rank:"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true" Enabled="false" Width="120px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="RadLabel1" runat="server" Text="Last Wage Drawn:"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="txtlastwages" runat="server" CssClass="input" MaxLength="12" Width="120px"
                                DecimalPlace="2" />
                        </td>
                    </tr>
                    <tr>

                        <td style="border-right: hidden">
                            <telerik:RadLabel ID="lblOccassionForReport" runat="server" Text="Contract Period :"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="txtContractPeriod" runat="server" CssClass="input" IsInteger="true" Width="90px" />
                            +/-
                            <eluc:Number ID="txtPlusMinusPeriod" runat="server" CssClass="input" MaxLength="3" Width="90px" />
                            (Months)
                        </td>
                        <td style="border-right: hidden">
                            <telerik:RadLabel ID="Literal20" runat="server" Text="Wages Agreed:"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="txtSalAgreed" runat="server" CssClass="input" MaxLength="12" Width="120px"
                                DecimalPlace="2" />
                            <asp:ImageButton ID="imgBtncrewagreed" runat="server" ToolTip="Crew Agreed"
                                ImageUrl="<%$ PhoenixTheme:images/Add.png%>" />
                        </td>
                        <td colspan="2">
                           <b><telerik:RadLabel ID="lbladdtionalwages" runat="server" Text=""></telerik:RadLabel></b> 
                        </td>

                    </tr>

                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVesselName" runat="server" Text="Agreed for type of Vessel :"></telerik:RadLabel>
                            <br />
                            <telerik:RadLabel ID="RadLabel2" runat="server" Text="Or Name of the Vessel :"></telerik:RadLabel>

                        </td>
                        <td> <eluc:Category ID="ucCategory" Width="240px" HardList='<%# PhoenixRegistersHard.ListHard(1,81)%>' runat="server" CssClass="dropdown_mandatory" HardTypeCode="81" />
                            <br />
                            <eluc:Vessel ID="ddlVessel" runat="server" AppendDataBoundItems="true" VesselsOnly="true" Entitytype="VSL" Width="240px"
                                ActiveVesselsOnly="true" CssClass="input" AppendItemPreSea="true" VesselList="<%#PhoenixRegistersVessel.ListVessel() %>" />
                            
                           
                            <%--   <eluc:VesselType ID="ucVesselType" runat="server" Width="240px" AppendDataBoundItems="true"
                                CssClass="input" />--%>
                        </td>
                        <td style="border-right: hidden">
                            <telerik:RadLabel ID="labelFrom" runat="server" Text="CBA - if Any"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Address ID="ddlUnion" runat="server" AppendDataBoundItems="true" CssClass="input"
                                AutoPostBack="true" AddressType="134" Width="180px" />
                        </td>

                        <td style="border-right: hidden">
                            <telerik:RadLabel ID="lblAppraisalDate" runat="server" Text="Travel Readiness Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date runat="server" ID="txtTraveldate" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">The contract period above, with operational clause ___M (+/-1), will reflect on the contract. To meet MLC requirements, expiry date shall be
                            <br />
                            printed on the contract & shall be of +1M from the date of departure from last International airport from India.</td>
                    </tr>
                </table>
                <table width="100%" cellspacing="0" cellpadding="1" border="1" style="width: 99%;" rules="all">
                    <tr>
                        <td>Courses to do (Please specify as per matrix): </td>
                        <td>
                            <telerik:RadTextBox ID="txtcoursestodo" runat="server" CssClass="input" Width="240px"></telerik:RadTextBox></td>
                        <td>DOA for courses:
                        </td>
                        <td>
                            <eluc:Date runat="server" ID="txtDOA" CssClass="input" />
                        </td>

                    </tr>
                    <tr>
                        <td>No of days required for training /courses:  </td>
                        <td>
                            <eluc:Number ID="txtnoofdays" runat="server" CssClass="input" IsInteger="true" Width="120px" />
                        </td>

                        <td>Briefed about courses regulations:</td>
                        <td>
                            <telerik:RadRadioButtonList ID="rdbisbriefed" runat="server" CssClass="input" Direction="Horizontal">
                                <Items>
                                    <telerik:ButtonListItem Text="Yes" Value="1" />
                                    <telerik:ButtonListItem Text="No" Value="0" />
                                </Items>
                            </telerik:RadRadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">OC9E Seafarer's Consent Letter and OC9F Medical Declaration by Seafarer explained and agreed to
                        </td>
                        <td>
                            <telerik:RadRadioButtonList ID="rdboc9e" runat="server" CssClass="input" Direction="Horizontal">
                                <Items>
                                    <telerik:ButtonListItem Text="Yes" Value="1" />
                                    <telerik:ButtonListItem Text="No" Value="0" />
                                </Items>
                            </telerik:RadRadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>Submit NEW KYC Form, if A/C is older than Two years at the time of joining.
                        </td>
                        <td>
                            <telerik:RadRadioButtonList ID="rdbnewkyc" runat="server" CssClass="input" Direction="Horizontal">
                                <Items>
                                    <telerik:ButtonListItem Text="Yes" Value="1" />
                                    <telerik:ButtonListItem Text="No" Value="0" />
                                </Items>
                            </telerik:RadRadioButtonList>
                        </td>
                        <td>HSBC / SCB Account  opened: 
                            <br />
                            If  already holding Account: A/C No
                        </td>
                        <td>
                            <telerik:RadRadioButtonList ID="rdbisbank" runat="server" CssClass="input" Direction="Horizontal">
                                <Items>
                                    <telerik:ButtonListItem Text="Yes" Value="1" />
                                    <telerik:ButtonListItem Text="No" Value="0" />
                                </Items>
                            </telerik:RadRadioButtonList>

                            <br />
                            <telerik:RadTextBox ID="txtAccountno" runat="server" CssClass="input"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <telerik:RadGrid RenderMode="Lightweight" ID="GvOfferLetter" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                CellSpacing="0" GridLines="None" OnItemCommand="GvOfferLetter_ItemCommand" OnNeedDataSource="GvOfferLetter_NeedDataSource" Height="90%"
                                OnItemDataBound="GvOfferLetter_ItemDataBound" OnUpdateCommand="GvOfferLetter_UpdateCommand" EnableViewState="true" GroupingEnabled="false" EnableHeaderContextMenu="true"
                                ShowFooter="false">
                                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
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
                                        <telerik:GridTemplateColumn HeaderText="Short Code" HeaderStyle-Width="10%">
                                            <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblShortCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDID") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Description" HeaderStyle-Width="80%">
                                            <HeaderStyle Wrap="true" HorizontalAlign="Left"></HeaderStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Order Sequence" HeaderStyle-Width="10%">
                                            <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblordersequence" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERNUMBER") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                    </Columns>
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                                </MasterTableView>
                                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                    <Scrolling AllowScroll="false" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" />
                                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                </ClientSettings>
                            </telerik:RadGrid></td>
                    </tr>
                </table>
                <table width="100%" cellspacing="0" cellpadding="1" border="1" style="width: 99%;" rules="all">
                    <tr>
                        <td colspan="4">
                            <telerik:RadTextBox ID="txtanyothercommitment" runat="server" CssClass="input" TextMode="MultiLine" Width="500px" Height="80px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">The above terms are in addition to the Standard Terms and Conditions discussed and agreed by the seafarer.
                            <br />
                            <br />
                            <br />
                            I have fully understood and agree to above.

                        </td>
                    </tr>
                    <tr>
                        <td>Name of Supdt
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtsupt" runat="server" CssClass="input" Width="80%"></telerik:RadTextBox>
                            <eluc:MCUser ID="Ucsupt" runat="server" Width="80%" emailrequired="false" designationrequired="false" />

                        </td>
                        <td>Date of Supdt signature

                        </td>
                        <td>
                            <eluc:Date runat="server" ID="txtsuptdate" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>Documentation (Please mention if any deficiencies and time required ):-
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtdocumentationmention" runat="server" TextMode="MultiLine" Width="180px" Height="50px" CssClass="input"></telerik:RadTextBox>
                        </td>
                        <td>Candidates is suitable for the vessel planned (except Flag state docs):-
                        </td>
                        <td>
                            <telerik:RadRadioButtonList ID="rdbissutiable" runat="server" CssClass="input" Direction="Horizontal">
                                <Items>
                                    <telerik:ButtonListItem Text="Yes" Value="1" />
                                    <telerik:ButtonListItem Text="No" Value="0" />
                                </Items>
                            </telerik:RadRadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>Following documents received from Seafarer:
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtdocrecived" runat="server" TextMode="MultiLine" Width="180px" Height="50px" CssClass="input"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
                <%--     <telerik:RadDockZone ID="RadDockZone3" runat="server" FitDocks="true" Orientation="Horizontal" Width="98%" BorderStyle="None">
                    <telerik:RadDock Width="99%" RenderMode="Lightweight" ID="RadDock3" runat="server" Title="Components Agreed with Crew" EnableAnimation="true"
                        EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false" EnableDrag="false">
                        <Commands>
                            <telerik:DockExpandCollapseCommand />
                        </Commands>
                        <ContentTemplate>
                           <telerik:RadGrid ID="gvCrew" Width="99%" EnableViewState="true" ShowFooter="true" OnItemCommand="gvCrew_ItemCommand" OnItemDataBound="gvCrew_ItemDataBound" runat="server" OnNeedDataSource="gvContract_NeedDataSource" ShowHeader="false">
                                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                                    AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                                    <Columns>
                                        <telerik:GridTemplateColumn>
                                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="35%"></ItemStyle>
                                            <FooterStyle HorizontalAlign="Left" Width="35%" />
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblContractCompIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTID")%>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblComponentName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME")%>' ToolTip='<%# DataBinder.Eval(Container, "DataItem.FLDDESCRIPTION")%>'>
                                                </telerik:RadLabel>
                                               
                                                <telerik:RadLabel ID="lblShortCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSHORTCODE")%>'></telerik:RadLabel>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <eluc:CrewComponents ID="ddlCrewComponentsAdd" runat="server" AppendDataBoundItems="true" Width="99%" CssClass="dropdown_mandatory" />
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="15%"></ItemStyle>
                                            <FooterStyle HorizontalAlign="Right" Width="12%" />
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <telerik:RadLabel ID="lblCompIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTID")%>'></telerik:RadLabel>
                                                <eluc:Number ID="txtAmount" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>' MaxLength="8" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <eluc:Number ID="txtAmountAdd" runat="server" CssClass="input_mandatory" MaxLength="8" Text="" />
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn>

                                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="15%"></ItemStyle>
                                            <FooterStyle HorizontalAlign="Left" Width="12%" />
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYCODE")%>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <eluc:Currency ID="ddlCurrencyEdit" runat="server" CssClass="dropdown_mandatory" Width="60px"
                                                    CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>' AppendDataBoundItems="true"
                                                    SelectedCurrency='<%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYID") %>' />

                                                <telerik:RadLabel ID="lblCurrencyEdit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <eluc:Currency ID="ddlCurrencyAdd" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" AutoPostBack="false"
                                                    CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>' SelectedCurrency="10" Width="60px" />
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="">
                                            <HeaderStyle Width="18%" HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container, "DataItem.FLDPAYABLE")%>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <telerik:RadLabel ID="lblPayableEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPAYABLE") %>'></telerik:RadLabel>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <telerik:RadLabel ID="lblPayableAdd" runat="server" Text=""></telerik:RadLabel>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%"></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit"><span class="icon"><i class="fas fa-edit"></i></span>
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
                                            <FooterStyle HorizontalAlign="Center" />
                                            <FooterTemplate>
                                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
                                                                      <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                                </asp:LinkButton>
                                            </FooterTemplate>
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
                                </MasterTableView>
                                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                </ClientSettings>
                            </telerik:RadGrid>
                        </ContentTemplate>
                    </telerik:RadDock>
                </telerik:RadDockZone>--%>
            </div>

            <eluc:Status runat="server" ID="ucStatus" />
            <asp:HiddenField runat="server" ID="hdnappraisalcomplatedyn" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
