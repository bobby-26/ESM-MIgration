<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOCIMF.aspx.cs" Inherits="CrewOCIMF" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>OCIMF</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
    <form id="frmRegistersRank" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="OcimfLogin" runat="server" OnTabStripCommand="OcimfLoginTabs_TabStripCommand"
                TabStrip="false"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="lstVessel" runat="server" AppendDataBoundItems="true" CssClass="input" OnTextChangedEvent="lstVessel_OnTextChanged"
                            AutoPostBack="true" VesselsOnly="true" Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOilMajor" runat="server" Text="Oil Major"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlOilMajor" runat="server" HardTypeCode="100" AppendDataBoundItems="true" CssClass="dropdown_mandatory" AutoPostBack="true" OnTextChangedEvent="ddlOilMajor_TextChangedEvent" Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPrinciple" runat="server" Visible="false" Text="Principle"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Address ID="ddlPrinciple" AddressType="128" runat="server" AppendDataBoundItems="true" Visible="false"
                            CssClass="input" Width="240px" />
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuCrewOCIMF" runat="server" OnTabStripCommand="CrewOCIMF_TabStripCommand" Visible="false"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvOCIMF" Height="45%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" OnItemCommand="gvOCIMF_ItemCommand" OnItemDataBound="gvOCIMF_ItemDataBound" EnableViewState="false" ShowFooter="false" ShowHeader="true" OnNeedDataSource="gvOCIMF_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" CommandItemDisplay="Top">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCrewType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWTYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Nationality">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNationality" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDNATIONALITY")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cert. Comp.">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCertComp" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCERTIFICATECOMPETENCY")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issuing Country">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIssuingCountry" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDISSUINGCOUNTRY")%>' ToolTip='<%# DataBinder.Eval(Container, "DataItem.FLDISSUINGCOUNTRY")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Admin. Accept">

                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAdminAccpt" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDADMINACCEPT")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Tanker Cert.">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTankerCert" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTANKERCERTIFICATION")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="STCW V Para.">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStcwPara" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSTCWPARA")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Radio Qual.">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRadioQual" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDRADIOQUALIFIED")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Operator">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblYrsOperator" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDYRSWITHOPERATOR")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblYrsRank" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDYRSRANKEXPERIENCE")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Tanker Type">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblYrsTankerType" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDYRSINTANKER")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="All Types">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblYrsAllType" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDYRSINALLTANKER")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Moths Tour">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDMONTHSTOUR")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Years Watch">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblYearsWatch" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDYEARSWATCH")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign-On">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJoinDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSIGNONDATE", "{0:yyyy-MM-dd}")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="English prof">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEngProf" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDENGLISHPROF")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add to SIRE" CommandName="Add" ID="cmdAdd" ToolTip="Add to SIRE">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>

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
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="3" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" ResizeGridOnColumnResize="false" />
                </ClientSettings>
            </telerik:RadGrid>
            <b>Sire Data</b>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvSire" Height="40%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" OnItemCommand="gvSire_ItemCommand" OnItemDataBound="gvSire_ItemDataBound" EnableViewState="false" ShowFooter="false" ShowHeader="true" OnNeedDataSource="gvSire_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" CommandItemDisplay="Top">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.rank").ToString().ToUpper() %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCrewType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.type") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.key") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblRankEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.rank").ToString().ToUpper() %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCrewTypeEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.type") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblKeyEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.key") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Nationality">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNationality" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.nationality").ToString().ToUpper()%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblNationalityEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.nationality").ToString().ToUpper()%>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cert. Comp.">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCertComp" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.certcomp").ToString().ToUpper()%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="lblCertCompEdit" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.certcomp").ToString().ToUpper()%>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issuing Country">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIssuingCountry" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.issuecountry").ToString().ToUpper()%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblIssuingCountryEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.issuecountry").ToString().ToUpper()%>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Admin. Accept">

                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAdminAccpt" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.adminaccept").ToString().ToUpper()%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblAdminAccptEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.adminaccept").ToString().ToUpper()%>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Tanker Cert.">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTankerCert" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.tankercert").ToString().ToUpper()%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblTankerCertEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.tankercert").ToString().ToUpper()%>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="STCW V Para.">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStcwPara" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.stcwvpara").ToString().ToUpper()%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblStcwParaEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.stcwvpara").ToString().ToUpper()%>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Radio Qual.">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRadioQual" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.radioqual").ToString().ToUpper()%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblRadioQualEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.radioqual").ToString().ToUpper()%>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Operator">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblYrsOperator" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.yearsoperator").ToString().ToUpper()%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtYrsOperator" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.yearsoperator").ToString().ToUpper()%>' DecimalPlace="1" IsPositive="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblYrsRank" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.yearsrank").ToString().ToUpper()%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtYrsRank" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.yearsrank").ToString().ToUpper()%>' DecimalPlace="1" IsPositive="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Tanker Type">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblYrsTankerType" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.yearstankertype").ToString().ToUpper()%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtYrsTankerType" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.yearstankertype").ToString().ToUpper()%>' DecimalPlace="1" IsPositive="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="All Types">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblYrsAllType" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.yearsalltankertypes").ToString().ToUpper()%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtYrsAllType" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.yearsalltankertypes").ToString().ToUpper()%>' DecimalPlace="1" IsPositive="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Years Watch">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblYrsWatch" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.YearsWatch").ToString().ToUpper()==""?"0.0":DataBinder.Eval(Container, "DataItem.YearsWatch").ToString().ToUpper()%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtYrsWatch" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.YearsWatch").ToString().ToUpper()==""?"0.0":DataBinder.Eval(Container, "DataItem.YearsWatch").ToString().ToUpper()%>' DecimalPlace="1" IsPositive="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign-On">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJoinDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.datejoinedvessel").ToString().ToUpper()%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="txtJoinDate" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.datejoinedvessel").ToString().ToUpper()%>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="English prof">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEngProf" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.englishprof").ToString().ToUpper()%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtEngProf" runat="server" CssClass="gridinput" Text='<%# DataBinder.Eval(Container, "DataItem.englishprof").ToString().ToUpper()%>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
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
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="3" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" ResizeGridOnColumnResize="false" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
