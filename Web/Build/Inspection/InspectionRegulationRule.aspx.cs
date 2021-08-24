using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Inspection_InspectionRegulationRule : PhoenixBasePage
{
    Guid? RegulationId;
    string lblVesselType = "Vessel Type";
    string lblSurveyType = "Survey";
    string lblYearBuild = "Year Built";
    string lblCertificate = "Certificate";
    string lblDocking = "Next Docking";
    string lblKeelaid = "Keel Laid";
    string regulationStatus = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        regulationStatus = Request.QueryString["Status"];
        if (string.IsNullOrWhiteSpace(Request.QueryString["RegulationId"]) == false)
        {
            RegulationId = new Guid(Request.QueryString["RegulationId"]);
            gvNewRule.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            gvNewAttribute.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            GetRegulationDetail();
        }
        if (IsPostBack == false)
        {
            gvNewRule.Rebind();
            ShowToolBar();
        }
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("javascript:openNewWindow('MoreInfo','','"+ Session["sitepath"] + "/Inspection/InspectionRegulationRuleAdd.aspx?RegulationId="+ RegulationId.Value + "'); return false;", "Add New Rule", "<i class=\"fa fa-plus-circle\"></i>", "ADDRULE");
        RuleTab.AccessRights = this.ViewState;
        RuleTab.MenuList = toolbar.Show();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvNewAttribute.Rebind();
            gvNewRule.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ShowToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);     
       // toolbar.AddImageLink(String.Format("javascript:openNewWindow('MoreInfo','','{0}/Inspection/InspectionRegulationAttributeAdd.aspx?RegulationId={1}&RuleId={2} '); return false;", Session["sitepath"], RegulationId.Value, ViewState["RuleId"]), "Add New Attribute", "", "ADDNEWATTRIBUTE", ToolBarDirection.Right);
        toolbar.AddButton("Action Plan", "ACTION", ToolBarDirection.Right);
        toolbar.AddButton("Apply Rule", "APPLY", ToolBarDirection.Right);
        if (regulationStatus == "Issued")
        {
            toolbar.AddButton("Apply Rule for New Vessel", "APPLYNEWVESSEL", ToolBarDirection.Right);
        }

        gvNewRuleTabStrip.AccessRights = this.ViewState;
        gvNewRuleTabStrip.MenuList = toolbar.Show();
    }
    private void GetRegulationDetail()
    {
        DataSet ds = PhoenixInspectionNewRegulation.RegulationDetail(RegulationId);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            txtRegulationTitle.Text = row["FLDTITLE"].ToString();
            txtIssuedDate.Text = Convert.ToDateTime(row["FLDISSUEDATE"]).ToString("dd-MM-yyyy");
            txtIssuedBy.Text = row["FLDISSUEDBYNAME"].ToString();
            txtDescription.Text = row["FLDDESCRIPTION"].ToString();
            txtDescription.Enabled = false;
        }
    }
    private void ShowError(string Message)
    {
        ucError.ErrorMessage = Message;
        ucError.Visible = true;
    }
    private DataSet GetRuleList(ref int iRowCount, ref int iTotalPageCount)
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionNewRegulation.RuleList(RegulationId, int.Parse(ViewState["PAGENUMBER"].ToString()), gvNewRule.PageSize, ref iRowCount, ref iTotalPageCount);
        return ds;
    }
    private DataSet GetAttributeList(Guid? RuleId, Guid? RegulationId, ref int iRowCount, ref int iTotalPageCount)
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionNewRegulation.RuleAttributeList(RuleId, RegulationId, int.Parse(ViewState["PAGENUMBER"].ToString()), gvNewAttribute.PageSize, ref iRowCount, ref iTotalPageCount);
        return ds;
    }
    protected void gvNewRuleTabStrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName == "EXCEL")
            {
                ShowExcel();
            }
            else if (CommandName == "BACK")
            {
                Response.Redirect("../Inspection/InspectionRegulation.aspx", false);
            }
            else if (CommandName == "APPLYNEWVESSEL")
            {
                RegulationVesselPopulateForNewVessel();
                ucStatus.Text = "Applied Successfully";
            }
            else if (CommandName == "APPLY")
            {
                RegulationVesselPopulate();
                ucStatus.Text = "Applied Successfully";
            }
            else if (CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            else if (CommandName == "ACTION")
            {
                Response.Redirect("../Inspection/InspectionRegulationActionPlan.aspx?REGULATIONID=" + Request.QueryString["RegulationId"], false);
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }
    private string[] getAlColumns()
    {
        string[] alColumns = { "FLDAPPLY", "FLDRULESORTORDER", "FLDVESSELTYPE", "FLDYEARBUILT", "FLDISBEFORE", "FLDDUEDATE" };
        return alColumns;
    }
    private string[] getAlCaptions()
    {
        string[] alCaptions = { "Apply", "Sort Order", "VesselType", "Year Built", "Before/After", "Due Date" };
        return alCaptions;
    }
    private void PrintReport(DataSet ds)
    {
        string[] alColumns = getAlColumns();
        string[] alCaptions = getAlCaptions();
        General.SetPrintOptions("gvNewRule", "Regulation Rule", alCaptions, alColumns, ds);
    }
    private void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string fileName = "RegulationRule";
        string reportName = "Regulation Rule";

        DataSet ds = new DataSet();
        string[] alColumns = getAlColumns();
        string[] alCaptions = getAlCaptions();
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        gvNewRule.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        ds = GetRuleList(ref iRowCount, ref iTotalPageCount);
        Response.AddHeader(String.Format("Content-Disposition", "attachment; filename={0}"), fileName);
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write(String.Format("<td><h3>{0}</h3></td>", reportName));
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    private void RegulationVesselPopulate()
    {
        int usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        PhoenixInspectionNewRegulation.RuleAppliedVesselPopulate(RegulationId, usercode);
    }

    private void RegulationVesselPopulateForNewVessel()
    {
        int usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        PhoenixInspectionNewRegulation.RuleAppliedVesselPopulateNewVessel(usercode);
    }
    #region Grid Events

    protected void gvNewRule_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvNewRule.CurrentPageIndex + 1;
        DataSet ds = GetRuleList(ref iRowCount, ref iTotalPageCount);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["RuleId"] = ds.Tables[0].Rows[0]["FLDRULEID"];
        }
        gvNewRule.DataSource = ds;
        gvNewRule.VirtualItemCount = iRowCount;
        PrintReport(ds);
    }
    protected void gvNewRule_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        
        if (e.Item is GridEditableItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            LinkButton deleteBtn = (LinkButton)e.Item.FindControl("cmdDelete");
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (deleteBtn != null)
            {
                deleteBtn.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                deleteBtn.Visible = SessionUtil.CanAccess(this.ViewState, deleteBtn.CommandName);
            }
            if (eb != null)
            {
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
            }
            if ((e.Item is GridEditableItem && e.Item.IsInEditMode))
            {
                RadRadioButtonList chkApply = (RadRadioButtonList)e.Item.FindControl("chkApplyEdit");
                if (ViewState["Apply"].ToString() == "Yes")
                {
                    chkApply.SelectedIndex = 0;
                }
                else
                {
                    chkApply.SelectedIndex = 1;
                }
            }
            LinkButton AddAttribute = (LinkButton)e.Item.FindControl("cmdAddAttribute");
            if (AddAttribute != null)
                AddAttribute.Attributes.Add("onclick",
                    "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRegulationAttributeAdd.aspx?RegulationId="+ RegulationId.Value + "&RuleId="+ dr["FLDRULEID"] + "');return true;");
        }
    }
    protected void gvNewRule_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "EDIT")
            {
                ViewState["Apply"] = ((RadLabel)e.Item.FindControl("lblApply")).Text;
            }
            if (e.CommandName.ToUpper() == "SELECT" || e.CommandName.ToUpper() == "EDIT" || e.CommandName.ToUpper() == "DELETE")
            {
                string ruleID = ((RadTextBox)e.Item.FindControl("lblRuleId")).Text;
                ViewState["RuleId"] = ruleID;
                gvNewAttribute.Rebind();
                ShowToolBar(); // for Creation of New Attribute selected rule id is required
            }
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                //RadTextBox ruleName = (RadTextBox)e.Item.FindControl("txtRuleNameAdd");
                //RadButtonList apply = (RadButtonList)e.Item.FindControl("chkApplyAdd");
                //if (ValidateRule(ruleName.Text, apply.SelectedValue))
                //{
                //    PhoenixInspectionNewRegulation.RuleInsert(RegulationId.Value, ruleName.Text, PhoenixSecurityContext.CurrentSecurityContext.UserCode, Convert.ToBoolean(apply.SelectedValue));
                //    gvNewRule.Rebind();
                //}
                //else
                //{
                //    throw new ArgumentException("");
                //}

            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                RadTextBox ruleName = (RadTextBox)e.Item.FindControl("txtRuleNameEdit");
                RadButtonList apply = (RadButtonList)e.Item.FindControl("chkApplyEdit");
                PhoenixInspectionNewRegulation.RuleUpdate(new Guid(ViewState["RuleId"].ToString()), ruleName.Text, PhoenixSecurityContext.CurrentSecurityContext.UserCode, Convert.ToBoolean(apply.SelectedValue));
                gvNewRule.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                LinkButton delete = (LinkButton)e.Item.FindControl("cmdDelete");
                PhoenixInspectionNewRegulation.RuleDelete(RegulationId, new Guid(ViewState["RuleId"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                gvNewRule.Rebind();
                gvNewAttribute.Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }
    protected void gvNewAttribute_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        Guid? ruleId = null;
        if (ViewState["RuleId"] != null)
        {
            ruleId = new Guid(ViewState["RuleId"].ToString());
        }
        DataSet ds = GetAttributeList(ruleId, RegulationId, ref iRowCount, ref iTotalPageCount);
        gvNewAttribute.DataSource = ds;
        gvNewAttribute.VirtualItemCount = iRowCount;
    }
    protected void gvNewAttribute_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "EDIT")
            {
                string Attibute = ((RadLabel)e.Item.FindControl("lblAttributeId")).Text;
                string AttibuteName = ((RadLabel)e.Item.FindControl("lblFieldName")).Text;
                string displayCode = ((RadLabel)e.Item.FindControl("lblCode")).Text;
                string displayName = ((RadLabel)e.Item.FindControl("lblName")).Text;
                ViewState["Condition"] = ((RadLabel)e.Item.FindControl("lblCondition")).Text;
                ViewState["Code"] = displayCode;
                ViewState["Name"] = displayName;
                ViewState["AttributeID"] = Attibute;
                ViewState["AttributeName"] = ((RadLabel)e.Item.FindControl("lblFieldName")).Text;
            }

            int usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                //string value = "";
                //string DispalyCode = "";
                //string DispalyName = "";
                //RadDropDownList condition = (RadDropDownList)e.Item.FindControl("ddlConditionAdd");
                //RadComboBox attribute = (RadComboBox)e.Item.FindControl("ddlFieldNameAdd");
                //RadTextBox valueField = (RadTextBox)e.Item.FindControl("txtValueAdd");
                //RadTextBox sort = (RadTextBox)e.Item.FindControl("txtSortOrderAdd");
                //RadTextBox tetJobId = (RadTextBox)e.Item.FindControl("txtVesselTypeIdAdd");
                //RadDatePicker ddlYear = (RadDatePicker)e.Item.FindControl("ddlYearAdd");
                //RadTextBox txtsurveyId = (RadTextBox)e.Item.FindControl("txtSurveyIDAdd");
                //RadTextBox txtcertificateId = (RadTextBox)e.Item.FindControl("txtCertificateIDAdd");
                ////UserControlDate DueDate = (UserControlDate)e.Item.FindControl("txtDateAdd");
                //RadRadioButtonList earlierLater = (RadRadioButtonList)e.Item.FindControl("chkEarlierLaterAdd");
                //RadRadioButtonList beforeAfter = (RadRadioButtonList)e.Item.FindControl("chkBeforeAfterAdd");

                //if (attribute.SelectedItem.Text == lblYearBuild)
                //{
                //    value = ddlYear.SelectedDate.Value.ToString("yyyy-MM-dd");
                //    condition.SelectedText = beforeAfter.SelectedValue;
                //}
                //else if (attribute.SelectedItem.Text == lblSurveyType)
                //{
                //    value = txtsurveyId.Text;
                //    DispalyCode = ((RadTextBox)e.Item.FindControl("txtSurveyCodeAdd")).Text;
                //    DispalyName = ((RadTextBox)e.Item.FindControl("txtSurveyAdd")).Text;
                //    condition.SelectedText = earlierLater.SelectedValue;
                //}
                //else if (attribute.SelectedItem.Text == lblCertificate)
                //{
                //    value = txtcertificateId.Text;
                //    DispalyCode = ((RadTextBox)e.Item.FindControl("txtCerticiateCodeAdd")).Text;
                //    DispalyName = ((RadTextBox)e.Item.FindControl("txCertificateAdd")).Text;
                //    condition.SelectedText = earlierLater.SelectedValue;
                //}
                //else if (attribute.SelectedItem.Text == lblDocking)
                //{
                //    value = ddlYear.SelectedDate.Value.ToString("yyyy-MM-dd");
                //    condition.SelectedText = earlierLater.SelectedValue;
                //}
                //else
                //{
                //    value = valueField.Text;
                //}

                //if (ViewState["RuleId"] == null || string.IsNullOrWhiteSpace(ViewState["RuleId"].ToString()))
                //{
                //    throw new ArgumentException("please select a rule and add attributes");
                //}
                //Guid? ruleId = new Guid(ViewState["RuleId"].ToString());
                //if (ValidateAttribute(sort.Text, attribute.SelectedItem.Value, condition.SelectedText, value.ToString()))
                //{
                //    int sortorder = Convert.ToInt32(sort.Text);
                //    PhoenixInspectionNewRegulation.RuleAttributeInsert(RegulationId, ruleId, new Guid(attribute.SelectedItem.Value), value, sortorder, condition.SelectedText, usercode, DispalyName, DispalyCode);
                //    //RegulationVesselPopulate();
                //    gvNewAttribute.Rebind();
                //}
                //else
                //{
                //    throw new ArgumentException("");
                //}
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                //string value;
                //string DispalyCode = "";
                //string DispalyName = "";
                //RadDropDownList condition = (RadDropDownList)e.Item.FindControl("ddlConditionEdit");
                //RadDropDownList attribute = (RadDropDownList)e.Item.FindControl("ddlFieldNameEdit");
                //RadTextBox valueField = (RadTextBox)e.Item.FindControl("txtValueEdit");
                //RadDatePicker Year = (RadDatePicker)e.Item.FindControl("ddlYearEdit");
                //RadTextBox vesselTypeId = (RadTextBox)e.Item.FindControl("txtVesselTypeIdEdit");
                //RadTextBox sort = (RadTextBox)e.Item.FindControl("txtSortOrderEdit");
                //Guid regulationId = new Guid(Request.QueryString["regulationId"]);
                //RadTextBox txtsurveyId = (RadTextBox)e.Item.FindControl("txtSurveyIDEdit");
                //RadTextBox txtcertificateId = (RadTextBox)e.Item.FindControl("txtCertificateIDEdit");
                //RadRadioButtonList earlierLater = (RadRadioButtonList)e.Item.FindControl("chkEarlierLaterEdit");
                //RadRadioButtonList beforeAfter = (RadRadioButtonList)e.Item.FindControl("chkBeforeAfterEdit");
                //UserControlDate DueDate = (UserControlDate)e.Item.FindControl("txtDateEdit");

                //int sortorder = Convert.ToInt32(sort.Text);
                //if (attribute.SelectedText == lblYearBuild)
                //{
                //    value = Year.SelectedDate.Value.ToString("yyyy-MM-dd");
                //    condition.SelectedText = beforeAfter.SelectedValue;
                //}
                //else if (attribute.SelectedText == lblSurveyType)
                //{
                //    value = txtsurveyId.Text;
                //    DispalyCode = ((RadTextBox)e.Item.FindControl("txtSurveyCodeEdit")).Text;
                //    DispalyName = ((RadTextBox)e.Item.FindControl("txtSurveyEdit")).Text;
                //    condition.SelectedText = earlierLater.SelectedValue;

                //}
                //else if (attribute.SelectedText == lblCertificate)
                //{
                //    value = txtcertificateId.Text;
                //    DispalyCode = ((RadTextBox)e.Item.FindControl("txtCerticiateCodeEdit")).Text;
                //    DispalyName = ((RadTextBox)e.Item.FindControl("txCertificateEdit")).Text;
                //    condition.SelectedText = earlierLater.SelectedValue;
                //}
                //else if (attribute.SelectedItem.Text == lblDocking)
                //{
                //    value = Year.SelectedDate.Value.ToString("yyyy-MM-dd");
                //    condition.SelectedText = earlierLater.SelectedValue;
                //}
                //else
                //{
                //    value = valueField.Text;
                //}

                //if (condition.SelectedText == "--Select--")
                //{
                //    ShowError("Please select condition");
                //}
                //else
                //{
                //    PhoenixInspectionNewRegulation.RuleAttributeUpdate(new Guid(ViewState["AttributeID"].ToString()), new Guid(attribute.SelectedItem.Value), value, sortorder, condition.SelectedText, usercode, DispalyName, DispalyCode);
                //    //RegulationVesselPopulate();
                //    gvNewAttribute.Rebind();
                //}
            }

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                TextBox attribute = (TextBox)e.Item.FindControl("txtAttributeId");
                Guid? attributeId = new Guid(attribute.Text);
                PhoenixInspectionNewRegulation.RuleAttributeDelete(RegulationId, attributeId, usercode);
                RegulationVesselPopulate();
                gvNewAttribute.Rebind();
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }
    protected void gvNewAttribute_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            //LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            //if (db != null)
            //{
            //    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            //}
            //LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            //if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
        }
        if ((e.Item is GridEditableItem && e.Item.IsInEditMode))
        {
            //GridEditableItem item = e.Item as GridEditableItem;
            //RadDropDownList fieldDropDown = (RadDropDownList)e.Item.FindControl("ddlFieldNameEdit");
            //fieldDropDown.DataSource = PhoenixInspectionNewRegulation.RuleAttributeDropDown();
            //fieldDropDown.DataTextField = "FLDATTRIBUTENAME";
            //fieldDropDown.DataValueField = "FLDATTRIBUTEREGISTERID";
            //fieldDropDown.DataBind();

            //displayControl(e.Item as GridEditableItem, ViewState["AttributeName"].ToString());

            //GridEditableItem editItem = (GridEditableItem)gvNewAttribute.MasterTableView.GetItems(GridItemType.EditItem)[0];

            //Image Vesselimg = (Image)editItem.FindControl("imgVesselTypeEdit");
            //Vesselimg.Attributes.Add("onclick", "javascript:return showPickList('spnPickListVesselTypeEdit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListVesselType.aspx', true);");

            //ImageButton Suveyimg = (ImageButton)editItem.FindControl("imgSurveyEdit");
            //Suveyimg.Attributes.Add("onclick", "javascript:return showPickList('spnPickListSurveyTypeEdit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListSurveyType.aspx', true);");

            //ImageButton Certificateimg = (ImageButton)editItem.FindControl("imgCertificateEdit");
            //Certificateimg.Attributes.Add("onclick", "javascript:return showPickList('spnPickListCertificateEdit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListCertificate.aspx', true);");
        }

        //if ((e.Item is GridFooterItem))
        //{
        //    GridFooterItem item = e.Item as GridFooterItem;
        //    RadComboBox fieldDropDown = (RadComboBox)e.Item.FindControl("ddlFieldNameAdd");
        //    fieldDropDown.DataSource = PhoenixInspectionNewRegulation.RuleAttributeDropDown();
        //    fieldDropDown.DataTextField = "FLDATTRIBUTENAME";
        //    fieldDropDown.DataValueField = "FLDATTRIBUTEREGISTERID";
        //    fieldDropDown.DataBind();

        //    Image img1 = (Image)e.Item.FindControl("imgVesselTypeAdd");
        //    img1.Attributes.Add("onclick", "javascript:return showPickList('spnPickListVesselTypeAdd', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListVesselType.aspx', true);");

        //    Image img2 = (Image)e.Item.FindControl("imgSurveyAdd");
        //    img2.Attributes.Add("onclick", "javascript:return showPickList('spnPickListSurveyTypeAdd', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListSurveyType.aspx', true);");

        //    Image certificate = (Image)e.Item.FindControl("imgCertificateAdd");
        //    certificate.Attributes.Add("onclick", "javascript:return showPickList('spnPickListCertificateAdd', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListCertificate.aspx', true);");
        //}

        if (e.Item is GridDataItem)
        {
            if (!e.Item.IsInEditMode)
            {
                string lblAttributeName = ((RadLabel)e.Item.FindControl("lblFieldName")).Text;
                RadLabel lblValue = (RadLabel)e.Item.FindControl("lblValue");
                RadLabel lblCode = (RadLabel)e.Item.FindControl("lblCode");
                RadLabel lblName = (RadLabel)e.Item.FindControl("lblName");
                RadLabel earlier = (RadLabel)e.Item.FindControl("lblEarlier");
                RadLabel before = (RadLabel)e.Item.FindControl("lblBefore");
                RadLabel condition = (RadLabel)e.Item.FindControl("lblCondition");

                if (lblAttributeName == lblSurveyType || lblAttributeName == lblVesselType || lblAttributeName == lblCertificate)
                {
                    lblValue.Visible = false;
                    lblCode.Visible = false;
                    lblName.Visible = true;

                    condition.Visible = false;
                    before.Visible = false;
                    earlier.Visible = true;

                    earlier.Text = condition.Text;
                    earlier.Text = condition.Text == ">" ? "Later" : "Earlier";

                }
                else if (lblAttributeName == lblDocking)
                {
                    lblValue.Visible = true;
                    lblCode.Visible = false;
                    lblName.Visible = false;

                    condition.Visible = false;
                    before.Visible = false;
                    earlier.Visible = true;

                    earlier.Text = condition.Text;
                    earlier.Text = condition.Text == ">" ? "Later" : "Earlier";
                    lblValue.Text = Convert.ToDateTime(lblValue.Text).ToString("dd-MM-yyyy");

                }
                else if (lblAttributeName == lblYearBuild)
                {
                    before.Visible = true;
                    condition.Visible = false;
                    earlier.Visible = false;
                    before.Text = condition.Text == "<" ? "Before" : "After";
                    lblValue.Text = Convert.ToDateTime(lblValue.Text).ToString("dd-MM-yyyy");
                }
                else if (lblAttributeName == lblKeelaid)
                {
                    before.Visible = true;
                    condition.Visible = false;
                    earlier.Visible = false;
                    before.Text = condition.Text == "<" ? "Before" : "After";
                    lblValue.Text = Convert.ToDateTime(lblValue.Text).ToString("dd-MM-yyyy");
                }
                else
                {
                    condition.Visible = true;
                    lblValue.Visible = true;
                    lblCode.Visible = false;
                    lblName.Visible = false;
                }

                LinkButton deleteBtn = (LinkButton)e.Item.FindControl("cmdAttributeDelete");
                if (deleteBtn != null) deleteBtn.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

                LinkButton editBtn = (LinkButton)e.Item.FindControl("cmdAttributeEdit");
                if (editBtn != null) editBtn.Visible = SessionUtil.CanAccess(this.ViewState, editBtn.CommandName);
                GridDataItem item = (GridDataItem)e.Item;
                string Attibute = ((RadLabel)e.Item.FindControl("lblAttributeId")).Text;
                if (Attibute != null)
                {
                    //editBtn.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + dtKey.Text + "&mod=Inspection&u=n'); return false;");
                    editBtn.Attributes.Add("onclick", String.Format("javascript:openNewWindow('MoreInfo','','{0}/Inspection/InspectionRegulationAttributeAdd.aspx?RegulationId={1}&RuleId={2}&AttributeId={3} '); return false;", Session["sitepath"], RegulationId.Value, ViewState["RuleId"], Attibute));
                }
            }
        }
    }
    #endregion
    private bool ValidateRule(string rulename, string apply)
    {
        bool validate = true;
        ucError.HeaderMessage = "Please provide the following required information";
        if (String.IsNullOrEmpty(rulename))
        {
            validate = false;
            ucError.ErrorMessage = "Please provide rule name";
        }

        if (String.IsNullOrEmpty(apply))
        {
            ucError.ErrorMessage = "Please select apply status";
            validate = false;
        }
        return validate;
    }
    private bool ValidateAttribute(string sortorder, string attrbute, string condition, string value)
    {
        bool validate = true;
        ucError.HeaderMessage = "Please provide the following required information";
        if (String.IsNullOrEmpty(sortorder))
        {
            validate = false;
            ucError.ErrorMessage = "Please provide sortorder";
        }
        if (String.IsNullOrEmpty(attrbute))
        {
            validate = false;
            ucError.ErrorMessage = "Please provide attribtue";
        }
        if (String.IsNullOrWhiteSpace(condition))
        {
            validate = false;
            ucError.ErrorMessage = "Please provide condition";
        }
        if (string.IsNullOrWhiteSpace(value))
        {
            validate = false;
            ucError.ErrorMessage = "Please provide value";
        }
        return validate;
    }
    protected void ddlFieldNameAdd_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        displayControl((GridFooterItem)gvNewAttribute.MasterTableView.GetItems(GridItemType.Footer)[0], e.Text);
    }
    protected void ddlFieldNameEdit_SelectedIndexChanged1(object sender, DropDownListEventArgs e)
    {
        displayControl((GridEditableItem)gvNewAttribute.MasterTableView.GetItems(GridItemType.EditItem)[0], e.Text);
    }
    private void displayControl(GridItem item, string context)
    {
        GridItem gridItem = item;
        bool isEditable = false;
        if (gridItem is GridEditableItem)
        {
            isEditable = true;
        }

        RadTextBox valueText = (RadTextBox)gridItem.FindControl(isEditable ? "txtValueEdit" : "txtValueAdd");
        RadTextBox txtVesselTypeCode = (RadTextBox)gridItem.FindControl(isEditable ? "txtVesselTypeCodeEdit" : "txtVesselTypeCodeAdd");
        RadTextBox tetVesselTypeName = (RadTextBox)gridItem.FindControl(isEditable ? "txtVesselTypeNameEdit" : "txtVesselTypeNameAdd");
        RadTextBox tetVesselTypeId = (RadTextBox)gridItem.FindControl(isEditable ? "txtVesselTypeIdEdit" : "txtVesselTypeIdAdd");
        RadTextBox txtSurveyCode = (RadTextBox)gridItem.FindControl(isEditable ? "txtSurveyCodeEdit" : "txtSurveyCodeAdd");
        RadTextBox tetSurveyName = (RadTextBox)gridItem.FindControl(isEditable ? "txtSurveyEdit" : "txtSurveyAdd");
        RadTextBox tetSurveyId = (RadTextBox)gridItem.FindControl(isEditable ? "txtSurveyIDEdit" : "txtSurveyIDAdd");
        RadTextBox txtCertificateCode = (RadTextBox)gridItem.FindControl(isEditable ? "txtCerticiateCodeEdit" : "txtCerticiateCodeAdd");
        RadTextBox tetCertificateName = (RadTextBox)gridItem.FindControl(isEditable ? "txCertificateEdit" : "txCertificateAdd");
        RadTextBox tetCertificateId = (RadTextBox)gridItem.FindControl(isEditable ? "txtCertificateIDEdit" : "txtCertificateIDAdd");
        RadDatePicker ddlYear = (RadDatePicker)gridItem.FindControl(isEditable ? "ddlYearEdit" : "ddlYearAdd");
        RadDropDownList condition = (RadDropDownList)gridItem.FindControl(isEditable ? "ddlConditionEdit" : "ddlConditionAdd");
        RadRadioButtonList earlier = (RadRadioButtonList)gridItem.FindControl(isEditable ? "chkEarlierLaterEdit" : "chkEarlierLaterAdd");
        RadRadioButtonList before = (RadRadioButtonList)gridItem.FindControl(isEditable ? "chkBeforeAfterEdit" : "chkBeforeAfterAdd");

        if (isEditable)
        {
            RadDropDownList con = (RadDropDownList)gridItem.FindControl("ddlConditionEdit");
            con.SelectedItem.Text = ViewState["Condition"].ToString();
            RadDropDownList cc = (RadDropDownList)gridItem.FindControl("ddlFieldNameEdit");
            cc.SelectedText = context;
            condition.SelectedItem.Text = ViewState["Condition"].ToString();
        }

        Image Vesselimg = (Image)gridItem.FindControl(isEditable ? "imgVesselTypeEdit" : "imgVesselTypeAdd");
        ImageButton Suveyimg = (ImageButton)gridItem.FindControl(isEditable ? "imgSurveyEdit" : "imgSurveyAdd");
        ImageButton Certificateimg = (ImageButton)gridItem.FindControl(isEditable ? "imgCertificateEdit" : "imgCertificateAdd");

        tetVesselTypeId.Attributes.Add("style", "visibility:hidden");
        tetCertificateId.Attributes.Add("style", "visibility:hidden");

        if (context == lblYearBuild)
        {
            if (isEditable)
            {
                before.SelectedValue = condition.SelectedText;
                ddlYear.SelectedDate = Convert.ToDateTime(valueText.Text);
            }

            ddlYear.Visible = true;
            valueText.Visible = false;
            txtVesselTypeCode.Visible = false;
            tetVesselTypeName.Visible = false;
            Vesselimg.Visible = false;
            txtSurveyCode.Visible = false;
            tetSurveyName.Visible = false;
            Suveyimg.Visible = false;
            //DateEdit.Visible = false;
            txtCertificateCode.Visible = false;
            tetCertificateName.Visible = false;
            Certificateimg.Visible = false;

            before.Visible = true;
            earlier.Visible = false;
            condition.Visible = false;
        }
        else if (context == lblSurveyType)
        {
            if (isEditable)
            {
                txtSurveyCode.Text = ViewState["Code"].ToString();
                tetSurveyName.Text = ViewState["Name"].ToString();
                earlier.SelectedValue = condition.SelectedText;
            }
            txtSurveyCode.Visible = true;
            tetSurveyName.Visible = true;
            Suveyimg.Visible = true;

            valueText.Visible = false;
            ddlYear.Visible = false;
            txtVesselTypeCode.Visible = false;
            tetVesselTypeName.Visible = false;
            Vesselimg.Visible = false;
            tetVesselTypeId.Visible = false;
            //DateEdit.Visible = false;
            txtCertificateCode.Visible = false;
            tetCertificateName.Visible = false;
            Certificateimg.Visible = false;

            before.Visible = false;
            earlier.Visible = true;
            condition.Visible = false;
        }
        else if (context == lblCertificate)
        {
            if (isEditable)
            {
                txtCertificateCode.Text = ViewState["Code"].ToString();
                tetCertificateName.Text = ViewState["Name"].ToString();
                earlier.SelectedValue = condition.SelectedText;
            }

            txtCertificateCode.Visible = true;
            tetCertificateName.Visible = true;
            Certificateimg.Visible = true;

            valueText.Visible = false;
            txtVesselTypeCode.Visible = false;
            tetVesselTypeName.Visible = false;
            Vesselimg.Visible = false;
            ddlYear.Visible = false;
            tetVesselTypeId.Visible = false;
            //DateEdit.Visible = false;
            txtSurveyCode.Visible = false;
            tetSurveyName.Visible = false;
            Suveyimg.Visible = false;

            before.Visible = false;
            earlier.Visible = true;
            condition.Visible = false;
        }
        else if (context == lblDocking)
        {
            ///*DateEdit.Visible =*/ true;
            if (isEditable)
            {
                earlier.SelectedValue = condition.SelectedText;
                ddlYear.SelectedDate = Convert.ToDateTime(valueText.Text);
            }
            ddlYear.Visible = true;

            txtCertificateCode.Visible = false;
            tetCertificateName.Visible = false;
            Certificateimg.Visible = false;
            valueText.Visible = false;
            txtVesselTypeCode.Visible = false;
            tetVesselTypeName.Visible = false;
            Vesselimg.Visible = false;
            tetVesselTypeId.Visible = false;
            txtSurveyCode.Visible = false;
            tetSurveyName.Visible = false;
            Suveyimg.Visible = false;

            before.Visible = false;
            earlier.Visible = true;
            condition.Visible = false;
        }
        else
        {
            valueText.Visible = true;

            //DateEdit.Visible = false;
            txtVesselTypeCode.Visible = false;
            tetVesselTypeName.Visible = false;
            Vesselimg.Visible = false;
            ddlYear.Visible = false;
            txtSurveyCode.Visible = false;
            tetSurveyName.Visible = false;
            Suveyimg.Visible = false;
            txtCertificateCode.Visible = false;
            tetCertificateName.Visible = false;
            Certificateimg.Visible = false;

            before.Visible = false;
            earlier.Visible = false;
            condition.Visible = true;
        }
    }
}