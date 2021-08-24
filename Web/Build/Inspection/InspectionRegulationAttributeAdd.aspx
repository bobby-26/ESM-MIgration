<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRegulationAttributeAdd.aspx.cs" Inherits="Inspection_InspectionRegulationAttributeAdd" %>

<%@ Register TagPrefix="eluc" TagName="Calendar" Src="~/UserControls/UserControlCalenderYear.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Regulation Attribute Add</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
            <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
            <telerik:RadCodeBlock ID="RadCodeBlock3" runat="server">
                <eluc:TabStrip ID="NewAttribute" runat="server" OnTabStripCommand="NewAttribute_TabStripCommand"></eluc:TabStrip>
            </telerik:RadCodeBlock>
        
            <table cellspacing="1" width="100%">
                <tr>
                    <br />
                </tr>
                <tr>
                         <td>
                             <telerik:RadLabel RenderMode="Lightweight" ID="lblSortOrder" runat="server" Text="Sort Order"></telerik:RadLabel>
                         </td>
                         <td>
                             <telerik:RadTextBox RenderMode="Lightweight" ID="txtSortOrder" runat="server" Enabled="true"></telerik:RadTextBox>
                         </td>
                    </tr>
                    <tr>
                          <td>
                             <telerik:RadLabel RenderMode="Lightweight" ID="lblAttribute" runat="server" Text="Attribute"></telerik:RadLabel>
                         </td>
                         <td>
                                <telerik:RadComboBox ID="ddlFieldNameAdd" AutoPostBack="true" OnSelectedIndexChanged="ddlFieldNameAdd_SelectedIndexChanged" ExpandDirection="Up" runat="server" RenderMode="Lightweight"></telerik:RadComboBox>
                         </td>
                    </tr>
                      <tr>
                          <td>
                             <telerik:RadLabel RenderMode="Lightweight" ID="lblCondition" runat="server" Text="Condition"></telerik:RadLabel>
                         </td>
                         <td>
                          <telerik:RadDropDownList ID="ddlConditionAdd" ExpandDirection="Up" runat="server" RenderMode="Lightweight">
                                <Items>
                                    <telerik:DropDownListItem Text="--Select--" Value="0" />
                                    <telerik:DropDownListItem Text="=" Value="1" />
                                    <telerik:DropDownListItem Text=">" Value="2" />
                                    <telerik:DropDownListItem Text=">=" Value="3" />
                                    <telerik:DropDownListItem Text="<" Value="4" />
                                    <telerik:DropDownListItem Text="<=" Value="5" />
                                </Items>
                            </telerik:RadDropDownList>
                             <telerik:RadRadioButtonList ID="chkEarlierLaterAdd" Visible="false" runat="server" AutoPostBack="false" Direction="Horizontal">
                                <Items>
                                    <telerik:ButtonListItem Text="Earlier" Value="<"/>
                                    <telerik:ButtonListItem Text="Later" Value=">"/>
                                </Items>
                            </telerik:RadRadioButtonList>
                            <telerik:RadRadioButtonList ID="chkBeforeAfterAdd" Visible="false" runat="server" AutoPostBack="false" Direction="Horizontal">
                                <Items>
                                    <telerik:ButtonListItem Text="Before" Value="<"/>
                                    <telerik:ButtonListItem Text="After" Value=">"/>
                                </Items>
                            </telerik:RadRadioButtonList>
                         </td>
                    </tr>
                   <tr>
                          <td>
                             <telerik:RadLabel RenderMode="Lightweight" ID="lblValue" runat="server" Text="Value"></telerik:RadLabel>
                         </td>
                         <td>
                              <telerik:RadTextBox ID="txtValueAdd" Visible="true" runat="server" RenderMode="Lightweight"></telerik:RadTextBox>

<%--                                 OnItemDataBound="RadMCUser_ItemDataBound"
                                OnItemsRequested="RadMCUser_ItemsRequested"
                                OnTextChanged="RadMCUser_TextChanged"--%>

                       


                         <%--   <span id="spnPickListVesselTypeAdd"> 
                                <telerik:RadTextBox Visible="false" ID="txtVesselTypeCodeAdd" runat="server" CssClass="input readonlytextbox" MaxLength="20"
                                    ReadOnly="false" Width="60px">
                                </telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtVesselTypeNameAdd" Visible="false" runat="server" CssClass="input readonlytextbox" MaxLength="20"
                                    ReadOnly="false" Width="210px">
                                </telerik:RadTextBox>
                                <asp:ImageButton ID="imgVesselTypeAdd" Visible="false" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                    ImageAlign="AbsMiddle" Text=".."  />
                                <telerik:RadTextBox ID="txtVesselTypeIdAdd" runat="server" CssClass="hidden" Width="10px"></telerik:RadTextBox>
                            </span>


                            <span id="spnPickListSurveyTypeAdd">
                                <telerik:RadTextBox Visible="false" ID="txtSurveyCodeAdd" runat="server" CssClass="input readonlytextbox" MaxLength="20"
                                    ReadOnly="false" Width="60px">
                                </telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtSurveyAdd" Visible="false" runat="server" CssClass="input readonlytextbox" MaxLength="20"
                                    ReadOnly="false" Width="210px">
                                </telerik:RadTextBox>
                                <asp:ImageButton ID="imgSurveyAdd" Visible="false" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                    ImageAlign="AbsMiddle" Text=".."  />
                                <telerik:RadTextBox ID="txtSurveyIDAdd" runat="server" CssClass="hidden" Width="10px"></telerik:RadTextBox>
                            </span>

                            <span id="spnPickListCertificateAdd">
                                <telerik:RadTextBox Visible="false" ID="txtCerticiateCodeAdd" runat="server" CssClass="input readonlytextbox" MaxLength="20"
                                    ReadOnly="false" Width="60px">
                                </telerik:RadTextBox>
                                <telerik:RadTextBox ID="txCertificateAdd" Visible="false" runat="server" CssClass="input readonlytextbox" MaxLength="20"
                                    ReadOnly="false" Width="210px">
                                </telerik:RadTextBox>
                                <asp:ImageButton ID="imgCertificateAdd" Visible="false" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                    ImageAlign="AbsMiddle" Text=".." />
                                <telerik:RadTextBox ID="txtCertificateIDAdd" runat="server" CssClass="hidden" Width="10px"></telerik:RadTextBox>
                            </span>--%>
                            <telerik:RadDatePicker  ID="ddlYearAdd" Visible="false" runat="server" ExpandDirection="Up"></telerik:RadDatePicker>


                                
                     <%--      <ItemTemplate>
                                         <telerik:RadLabel RenderMode="Lightweight" ID="radLabel2" runat="server"> <%# DataBinder.Eval(Container.DataItem, "FLDSURVEYTYPEID") %></telerik:RadLabel>
                            </ItemTemplate>
                         --%>
             
                            <telerik:RadComboBox RenderMode="Lightweight" AutoPostBack="true" Visible="false" runat="server" ID="RadComboCertificate" Width="200" Height="200"  
                            ShowMoreResultsBox="false"
                            Filter="Contains" MarkFirstMatch="true"
                            EmptyMessage="select from the dropdown or type Name" 
                            OnSelectedIndexChanged="RadMCUser_TextChanged"
                            HighlightTemplatedItems="true" DropDownAutoWidth="Enabled" >
                            </telerik:RadComboBox>

                            <telerik:RadLabel RenderMode="Lightweight" ID="lblComboSurveyType" Visible="false" runat="server" Text="Survey Type"></telerik:RadLabel>


                            <telerik:RadComboBox RenderMode="Lightweight" AutoPostBack="true" Visible="false" runat="server" ID="RadComboSurveyType" Width="200" Height="200"  
                            DataTextField="FLDSURVEYTYPENAME" 
                            ShowMoreResultsBox="false"
                            Filter="Contains" MarkFirstMatch="true"
                            EmptyMessage="select from the dropdown or type Name" 
                            DataValueField="FLDSURVEYTYPEID" HighlightTemplatedItems="true" DropDownAutoWidth="Enabled" >
                            </telerik:RadComboBox>


                             
                          <telerik:RadComboBox RenderMode="Lightweight" AutoPostBack="true" Visible="false" runat="server" ID="RadComboSurvey" Width="200" Height="200"  
                            DataTextField="FLDCERTIFICATENAME" 
                            ShowMoreResultsBox="false"
                            Filter="Contains" MarkFirstMatch="true"
                            EmptyMessage="select from the dropdown or type Name" 
                            DataValueField="FLDCERTIFICATEID" HighlightTemplatedItems="true" DropDownAutoWidth="Enabled" >
                            </telerik:RadComboBox>
                         </td>
                    </tr>
            </table>
            <eluc:Status ID="ucStatus" runat="server" />
        </div>
    </form>
</body>
</html>
