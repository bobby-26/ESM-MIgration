<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewAppraisalPromotion.aspx.cs"
    Inherits="CrewAppraisalPromotion" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Promotion" Src="~/UserControls/UserControlPromotionEvaluationItem.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Appraisal Activity</title>
       <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
      <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewAppraisalPromotion" runat="server">
    <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
  <telerik:RadAjaxPanel ID="panel1" runat="server">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            
                    <%--<eluc:Title runat="server" ID="Appraisalactivity" Text="Promotion Questions" ShowMenu="false">
                    </eluc:Title>--%>
                  <%--  <eluc:TabStrip ID="MenuPersonalPromotion" runat="server"></eluc:TabStrip>--%>
            
              <telerik:RadGrid ID="gvCrewPromotionAppraisal" runat="server" AutoGenerateColumns="False"
                        Font-Size="11px" Width="100%" CellPadding="3" OnItemDataBound="gvCrewPromotionAppraisal_ItemDataBound"
                        Style="margin-bottom: 0px" OnItemCommand="gvCrewPromotionAppraisal_ItemCommand"
                        ShowHeader="true" EnableViewState="false"   AllowPaging="true" AllowCustomPaging="true" GridLines="None" 
                        OnNeedDataSource="gvCrewPromotionAppraisal_NeedDataSource" RenderMode="Lightweight"
                        GroupingEnabled="false" EnableHeaderContextMenu="true">
                       
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                    HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" >
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="RadLabel1" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />			        
                            <Columns> 

                            <telerik:GridTemplateColumn HeaderText="Evaluation Item">
                                <HeaderStyle  Width="45%"></HeaderStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblAppraisalPromotionId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAPPRAISALPROMOTIONID")%>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblPromotionQuestionId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROMOTIONQUESTIONID")%>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblPromotionQuestion" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROMOTIONQUESTION")%>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Rating">
                                <HeaderStyle  Width="25%"></HeaderStyle>
                                <ItemStyle Width="10" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRating" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRESPONSE")%>' Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblRange" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRANGE")%>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number runat="server" ID="ucRatingEdit" CssClass="input_mandatory" MaxLength="2"  Width="100%"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESPONSE") %>' />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>

                         <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <FooterStyle Wrap="true" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center" ></ItemStyle>
                                <ItemTemplate>
                                     <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit"
                                    ToolTip="Edit">
                                         <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete">
                                         <span class="icon"><i class="fa fa-trash"></i></span>
                                </asp:LinkButton>
                                
                                </ItemTemplate>
                                 <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave"
                                    ToolTip="Save">
                                        <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel"
                                    ToolTip="Cancel">
                                        <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                                
                        </telerik:GridTemplateColumn>





                        </Columns>
                    <%--<PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />--%>
                </MasterTableView>
                    <%-- <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                         AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                         <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                         <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4"
                             ScrollHeight="" />
                         <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                     </ClientSettings>--%>
            </telerik:RadGrid>

               
                <hr />
                &nbsp;<b><telerik:RadLabel ID="lblPromotion" runat="server" Text="Promotion"></telerik:RadLabel></b>
                <br />
                <table width="100%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td style="width: 50%">
                            <telerik:RadLabel ID="lblHavetheTrainingstandardsfortankerrequirementsbeencompleted" runat="server" Text="Have the ‘Training standards for tanker’ requirements been completed?"></telerik:RadLabel>
                        </td>
                        <td style="width: 50%">
                            <telerik:RadCheckBox runat="server" ID="chkTanker" AutoPostBack="true" 
                                oncheckedchanged="chkbox_CheckedChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 50%">
                            <telerik:RadLabel ID="lblOnthebasisoftheabovedoyourecommendpromotiontonextrank" runat="server" Text="On the basis of the above, do you recommend promotion to next rank?"></telerik:RadLabel>
                        </td>
                        <td style="width: 50%">
                            <telerik:RadCheckBox runat="server" ID="chkRecommendPromotion"  AutoPostBack="true" oncheckedchanged="chkbox_CheckedChanged" />
                        </td>
                    </tr>
                </table>
                <eluc:Status runat="server" ID="ucStatus" />
                             
     
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>