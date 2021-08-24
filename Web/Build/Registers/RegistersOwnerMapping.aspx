<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersOwnerMapping.aspx.cs"
    Inherits="RegistersOwnerMapping" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="../UserControls/UserControlPool.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Numbers" Src="~/UserControls/UserControlNumber.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Owner Mapping</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersOwnerMapping" runat="server">

        <eluc:TabStrip ID="MenuOwnerMapping" runat="server" OnTabStripCommand="OwnerMapping_TabStripCommand" TabStrip="true"></eluc:TabStrip>

        <eluc:TabStrip ID="RegistersOwnerMappingMain" runat="server" OnTabStripCommand="RegistersOwnerMapping_TabStripCommand"></eluc:TabStrip>

        <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div id="divFind">
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPool" runat="server" Text="Pool"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Pool ID="ucPool" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReportingCurrency" runat="server" Text="Reporting Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Currency ID="ucCurrency" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            runat="server" />
                    </td>
                </tr>
                <tr runat="server" visible="false">
                    <td>
                        <telerik:RadLabel ID="lblVictuallingRate" runat="server" Text="Victualling Rate"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtVictualRate" CssClass="input_mandatory txtNumber"></asp:TextBox>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderVictualRate" runat="server"
                            TargetControlID="txtVictualRate" Mask="9,999.99" MaskType="Number" InputDirection="RightToLeft">
                        </ajaxToolkit:MaskedEditExtender>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBondSubsidyAmount" runat="server" Text="Bond Subsidy Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="ucAmountAdd" runat="server" CssClass="input"
                            Mask="999.99" IsPositive="true" Width="120px" MaxLength="6"></eluc:Number>
                    </td>
                </tr>
                <tr>
                    <%--<td>
                    <telerik:Radlabel ID="lblGSTChargeable" runat="server" Text="GST Chargeable"></telerik:Radlabel>
                </td>
                <td>
                    <asp:CheckBox ID="chkTaxBasis" runat="server" />
                </td>
                <td>
                    <telerik:Radlabel ID="lblDiscount" runat="server" Text="Discount %"></telerik:Radlabel>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtDiscountPercent" CssClass="input txtNumber"></asp:TextBox>
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderDiscountPercent" runat="server"
                        TargetControlID="txtDiscountPercent" Mask="999.99" MaskType="Number" InputDirection="RightToLeft">
                    </ajaxToolkit:MaskedEditExtender>
                </td>--%>

                    <td>
                        <telerik:RadLabel ID="lblApprovalrequiredforSeniorOfficer" runat="server" Text="Approval required for Senior Officer"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBox ID="ChkApproved" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBudgetPeriod" runat="server" Text="Budget Period"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucBudgetPeriod" runat="server" CssClass="dropdown_mandatory" HardTypeCode="74"
                            ShortNameFilter="JAN,APL,VSD" AppendDataBoundItems="true" />
                    </td>

                </tr>
                <%-- <tr>
               <%-- <td>
                    <telerik:Radlabel ID="lblAirfareMarkup" runat="server" Text="Airfare Markup %"></telerik:Radlabel>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtMarkupAirfare" CssClass="input txtNumber"></asp:TextBox>
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderMarkupAirfare" runat="server"
                        TargetControlID="txtMarkupAirfare" Mask="999.99" MaskType="Number" InputDirection="RightToLeft">
                    </ajaxToolkit:MaskedEditExtender>
                </td>
               
            </tr>--%>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblViewparticularsofindividuals" runat="server" Text="View particulars of individuals"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBox ID="ChkCrewDetails" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblViewCrewListHistory" runat="server" Text="View Crew List History"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBox ID="ChkCrewListHistory" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMaxlimitpercent" runat="server" Text="Maximum Allowable % Increase from PO"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtMaxlimitpercent" CssClass="input txtNumber" Width="60px"></asp:TextBox>
                        <telerik:RadLabel ID="lblPercentage" runat="server" Text="%"></telerik:RadLabel>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderMaxlimitpercent" runat="server"
                            TargetControlID="txtMaxlimitpercent" Mask="999" MaskType="Number" InputDirection="RightToLeft">
                        </ajaxToolkit:MaskedEditExtender>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMaxlimitUSD" runat="server" Text="Maximum Allowable USD Increase from PO"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="ucMaxlimitUSD" CssClass="input" Style="text-align: right;"></asp:TextBox>
                        <ajaxToolkit:MaskedEditExtender ID="MaskNumber" runat="server" TargetControlID="ucMaxlimitUSD"
                            Mask="999,999,999.99" MaskType="Number" InputDirection="RightToLeft">
                        </ajaxToolkit:MaskedEditExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <font color="blue">*Lower of the 2 Maximum Allowable Increase
                            <br />
                            Limits will apply for invoice clearance</font>

                    </td>
                    <td></td>
                    <td>
                        <telerik:RadLabel ID="Radlabel1" runat="server" Text="PO Straight-through processing is allowed"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlpostraight" runat="server" CssClass="input_mandatory">
                            <Items>
                                <telerik:RadComboBoxItem Text="Yes" Value="1"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="No" Value="0"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>

                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="Radlabel2" runat="server" Text="Invoice by Shipped Qty Allowed"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlinvoiceshippedqtyallowed" runat="server">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="null"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Yes" Value="1"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="No" Value="0"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>

                    </td>
                    <td>
                        <telerik:RadLabel ID="Radlabel3" runat="server" Text="No of Days Allowed at Forwarder"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="ucnoofdays" runat="server"></eluc:Number>


                    </td>
                </tr>
            </table>
            <telerik:RadLabel ID="lblbillingparty" runat="server" Visible="false"></telerik:RadLabel>
        </div>
        <br />

        <div id="divGrid" style="position: relative; z-index: 0">
            <asp:GridView ID="gvOwnerMapping" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnRowDataBound="gvOwnerMapping_ItemDataBound"
                ShowFooter="false" ShowHeader="true">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                <Columns>
                    <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                    <asp:TemplateField>
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblBillingParty" runat="server" Text="Billing Party"></telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAddresscode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESSCODE") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblNameCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
