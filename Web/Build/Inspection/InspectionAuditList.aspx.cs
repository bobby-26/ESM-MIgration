using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionAuditList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionAuditList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvInspection')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('IndpecDetail','Add Inspection','Inspection/InspectionAuditDetailEdit.aspx')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            MenuRegistersInspection.AccessRights = this.ViewState;
            MenuRegistersInspection.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                    ViewState["COMPANYID"] = nvc.Get("QMS");

                btnconfirm.Attributes.Add("style", "display:none");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvInspection.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int typeid = 0;
        string docname = "";

        string[] alColumns = { "FLDCOMPANYNAME", "FLDINSPECTIONCATEGORYNAME", "FLDSHORTCODE", "FLDINSPECTIONNAME", "FLDACTIVEYN",
                                 "FLDFREQUENCYINMONTHS", "FLDADDTOSCHEDULE", "FLDOFFICEYNNAME" , "FLDWINDOWPERIODBEFORE" , "FLDWINDOWPERIODAFTER","FLDINSPECTIONTYPE"};
        string[] alCaptions = { "Company", "Category", "Short Code", "Name", "Active Y/N", "Frequency (in months)", "Add to Schedule",
                                  "Office Audit Y/N" , "Window Before" , "Window After","Type"};

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        typeid = Convert.ToInt32(PhoenixCommonRegisters.GetHardCode(1, 148, "AUD"));
        docname = "Audit / Inspection";

        DataSet ds = PhoenixInspection.InspectionSearch(
           General.GetNullableInteger(typeid.ToString())
         , General.GetNullableInteger(ucInspectionCategory.SelectedHard)
         , null
         , sortexpression, sortdirection
         , 1
         , iRowCount
         , ref iRowCount
         , ref iTotalPageCount
         , General.GetNullableInteger(ucExternalAuditType.SelectedHard)
         , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=" + docname + ".xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>" + docname + "</h3></td>");
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

    protected void RegistersInspection_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvInspection.SelectedIndexes.Clear();
        gvInspection.EditIndexes.Clear();
        gvInspection.DataSource = null;
        gvInspection.Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int typeid = 0;
        string docname = "";


        string[] alColumns = { "FLDCOMPANYNAME", "FLDINSPECTIONCATEGORYNAME", "FLDSHORTCODE", "FLDINSPECTIONNAME", "FLDACTIVEYN",
                                 "FLDFREQUENCYINMONTHS", "FLDADDTOSCHEDULE", "FLDOFFICEYNNAME" , "FLDWINDOWPERIODBEFORE" , "FLDWINDOWPERIODAFTER","FLDINSPECTIONTYPE"};
        string[] alCaptions = { "Company", "Category", "Short Code", "Name", "Active Y/N", "Frequency (in months)", "Add to Schedule",
                                  "Office Audit Y/N" , "Window Before" , "Window After","Type"};

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        typeid = Convert.ToInt32(PhoenixCommonRegisters.GetHardCode(1, 148, "AUD"));
        docname = "Audit / Inspection";

        DataSet ds = PhoenixInspection.InspectionSearch(
         General.GetNullableInteger(typeid.ToString())
         , General.GetNullableInteger(ucInspectionCategory.SelectedHard)
         , null
         , sortexpression, sortdirection
         , Int32.Parse(ViewState["PAGENUMBER"].ToString())
         , gvInspection.PageSize
         , ref iRowCount
         , ref iTotalPageCount
         , General.GetNullableInteger(ucExternalAuditType.SelectedHard)
         , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));


        General.SetPrintOptions("gvInspection", docname, alCaptions, alColumns, ds);

        gvInspection.DataSource = ds;
        gvInspection.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }

    private void InsertInspection(string inspectiontypeid, string inspectiontypecategoryid, string inspectionname,
        string shortcode, int activeyn, string effectivedate, string externalaudittype, int officeaudityn, string companyid,
        string frequencyinmonths, int addtoschedule, string windowbefore, string windowafter, string Type,string QuestionType)
    {
        PhoenixInspection.InsertInspection(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , Int16.Parse(inspectiontypeid)
            , Int16.Parse(inspectiontypecategoryid)
            , inspectionname
            , shortcode
            , activeyn
            , null
            , General.GetNullableInteger(externalaudittype)
            , officeaudityn
            , General.GetNullableInteger(companyid)
            , General.GetNullableInteger(frequencyinmonths)
            , addtoschedule
            , General.GetNullableInteger(windowbefore)
            , General.GetNullableInteger(windowafter)
            , General.GetNullableString(Type)
            , General.GetNullableInteger(QuestionType)
            );
    }

    private void UpdateInspection(Guid inspectionid, string inspectiontypeid, string inspectiontypecategoryid, string inspectionname,
        string shortcode, int activeyn, string effectivedate, string externalaudittype, int officeaudityn, string companyid,
        string frequencyinmonths, int addtoschedule, string windowbefore, string windowafter, string Type,string QuestionType)
    {
        PhoenixInspection.UpdateInspection(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , inspectionid
            , Int16.Parse(inspectiontypeid)
            , Int16.Parse(inspectiontypecategoryid)
            , inspectionname
            , shortcode
            , activeyn
            , null
            , General.GetNullableInteger(externalaudittype)
            , officeaudityn
            , General.GetNullableInteger(companyid)
            , General.GetNullableInteger(frequencyinmonths)
            , addtoschedule
            , General.GetNullableInteger(windowbefore)
            , General.GetNullableInteger(windowafter)
            , General.GetNullableString(Type)
            , General.GetNullableInteger(QuestionType)
            );
        ucStatus.Text = "Information updated";
    }

    private bool IsValidInspection(string inspectiontypeid, string inspectiontypecategoryid, string inspectionname, string shortcode,
        string effectivedate, string externalaudittype, string officeyn, string companyid, string frequency,string QuestionType)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(companyid) == null)
            ucError.ErrorMessage = "Company is required.";
        if (inspectiontypeid.Trim().Equals("Dummy") || inspectiontypeid.Trim().Equals(""))
            ucError.ErrorMessage = "Type is required.";
        if (inspectiontypecategoryid.Trim().Equals("Dummy") || inspectiontypecategoryid.Trim().Equals(""))
            ucError.ErrorMessage = "Category is required.";
        if (shortcode.Trim().Equals(""))
            ucError.ErrorMessage = "Short code is required.";
        if (inspectionname.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";
        if (General.GetNullableInteger(frequency) == null)
            ucError.ErrorMessage = "Frequency is required.";
        if (General.GetNullableInteger(QuestionType) == null)
            ucError.ErrorMessage = "QuestionType is required.";

        return (!ucError.IsError);
    }

    private void DeleteInspection(Guid inspectionid)
    {
        PhoenixInspection.DeleteInspection(PhoenixSecurityContext.CurrentSecurityContext.UserCode, inspectionid);
    }

    private void CopyInspection(Guid frominspectionid, Guid? toinspectionid, int? iscpoy)
    {
        PhoenixInspection.CopyInspection(PhoenixSecurityContext.CurrentSecurityContext.UserCode, frominspectionid, toinspectionid, iscpoy);
    }

    protected void CloseWindow(object sender, EventArgs e)
    {
        try
        {
            string Script = "";
            Script += "fnReloadList(null,'ifMoreInfo','keepopen');";
            UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

            if (ucCM.confirmboxvalue == 1)
            {
                String script = String.Format("javascript:fnReloadList('codehelp1');");

                if (ViewState["InspectionId"] != null)
                    CopyInspection(new Guid(ViewState["InspectionId"].ToString()), null, null);
                Rebind();

                ucStatus.Text = "Schedule has been updated as 'Completed'";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    public void ucInspectionCategoryEdit_changed(object sender, EventArgs e)
    {
        UserControlHard uch = (UserControlHard)sender;
        GridDataItem Item = (GridDataItem)uch.NamingContainer;
        //UserControlHard uch = (UserControlHard)(sender);
        //GridViewRow dg = (GridViewRow)(uch.Parent.Parent);
        UserControlHard ucInspectionCategoryEdit = (UserControlHard)(Item.FindControl("ucInspectionCategoryEdit"));
        UserControlHard ucExternalAuditTypeEdit = (UserControlHard)(Item.FindControl("ucExternalAuditTypeEdit"));

        if (ucInspectionCategoryEdit != null && ucExternalAuditTypeEdit != null && (ucInspectionCategoryEdit.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 144, "EXT")))
        {
            ucExternalAuditTypeEdit.Enabled = true;
            ucExternalAuditTypeEdit.CssClass = "dropdown_mandatory";
        }
        else
        {
            ucExternalAuditTypeEdit.SelectedHard = "";
            ucExternalAuditTypeEdit.Enabled = false;
            ucExternalAuditTypeEdit.CssClass = "input";
        }
    }

    public void ucInspectionCategoryAdd_changed(object sender, EventArgs e)
    {
        UserControlHard uch = (UserControlHard)sender;
        GridFooterItem Item = (GridFooterItem)uch.NamingContainer;
        UserControlHard ucInspectionCategoryAdd = (UserControlHard)Item.FindControl("ucInspectionCategoryAdd");
        UserControlHard ucExternalAuditTypeAdd = (UserControlHard)Item.FindControl("ucExternalAuditTypeAdd");

        if (ucInspectionCategoryAdd != null && ucExternalAuditTypeAdd != null && ucInspectionCategoryAdd.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 144, "EXT"))
        {
            ucExternalAuditTypeAdd.Enabled = true;
            //ucExternalAuditTypeAdd.CssClass = "dropdown_mandatory";
        }
        else
        {
            ucExternalAuditTypeAdd.SelectedHard = "";
            ucExternalAuditTypeAdd.Enabled = false;
            //ucExternalAuditTypeAdd.CssClass = "input";
        }
    }

    public void ucInspectionCategory_Changed(object sender, EventArgs e)
    {
        if (ucInspectionCategory.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 144, "EXT"))
        {
            ucExternalAuditType.Enabled = true;
        }
        else
        {
            ucExternalAuditType.Enabled = false;
            ucExternalAuditType.SelectedHard = "";
        }
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }

    protected void chkOfficeAuditYNAdd_Changed(object sender, EventArgs e)
    {
        RadCheckBox uch = (RadCheckBox)sender;
        GridFooterItem Item = (GridFooterItem)uch.NamingContainer;

        RadCheckBox chkOfficeAuditYNAdd = (RadCheckBox)Item.FindControl("chkOfficeAuditYNAdd");
        UserControlCompany ucCompanyAdd = (UserControlCompany)Item.FindControl("ucCompanyAdd");
        if (chkOfficeAuditYNAdd != null && ucCompanyAdd != null && chkOfficeAuditYNAdd.Checked == true)
        {
            ucCompanyAdd.CssClass = "dropdown_mandatory";
            ucCompanyAdd.Enabled = true;
        }
        else
        {
            ucCompanyAdd.SelectedCompany = "";
            ucCompanyAdd.Enabled = false;
            ucCompanyAdd.CssClass = "input";
        }
    }

    protected void gvInspection_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                int typeid = 0;
                typeid = Convert.ToInt32(PhoenixCommonRegisters.GetHardCode(1, 148, "AUD"));

                if (!IsValidInspection(typeid.ToString()
                        , ((UserControlHard)e.Item.FindControl("ucInspectionCategoryAdd")).SelectedHard
                        , ((RadTextBox)e.Item.FindControl("txtInspectionNameAdd")).Text
                        , ((RadTextBox)e.Item.FindControl("txtInspectionShortCodeAdd")).Text
                        , ((UserControlDate)e.Item.FindControl("ucEffectiveDateAdd")).Text
                        , ((UserControlHard)e.Item.FindControl("ucExternalAuditTypeAdd")).SelectedHard
                        , (((RadCheckBox)e.Item.FindControl("chkOfficeAuditYNAdd")).Checked == true) ? "1" : "0"
                        , ((UserControlCompany)e.Item.FindControl("ucCompanyAdd")).SelectedCompany
                        , ((UserControlMaskNumber)e.Item.FindControl("txtFrequencyAdd")).Text
                        , (((RadComboBox)e.Item.FindControl("ddlQuestionTypeAdd")).SelectedValue)

                        ))
                {
                    ucError.Visible = true;
                    return;
                }

                InsertInspection
                    (
                           typeid.ToString()
                        , ((UserControlHard)e.Item.FindControl("ucInspectionCategoryAdd")).SelectedHard
                        , ((RadTextBox)e.Item.FindControl("txtInspectionNameAdd")).Text
                        , ((RadTextBox)e.Item.FindControl("txtInspectionShortCodeAdd")).Text
                        , (((RadCheckBox)e.Item.FindControl("chkActiveYNAdd")).Checked == true) ? 1 : 0
                        , ((UserControlDate)e.Item.FindControl("ucEffectiveDateAdd")).Text
                        , ((UserControlHard)e.Item.FindControl("ucExternalAuditTypeAdd")).SelectedHard
                        , (((RadCheckBox)e.Item.FindControl("chkOfficeAuditYNAdd")).Checked == true) ? 1 : 0
                        , ((UserControlCompany)e.Item.FindControl("ucCompanyAdd")).SelectedCompany
                        , ((UserControlMaskNumber)e.Item.FindControl("txtFrequencyAdd")).Text
                        , (((RadCheckBox)e.Item.FindControl("chkAddToScheduleYNAdd")).Checked == true) ? 1 : 0
                        , ((UserControlMaskNumber)e.Item.FindControl("txtWindowBeforeAdd")).Text
                        , ((UserControlMaskNumber)e.Item.FindControl("txtWindowAfterAdd")).Text
                        , ((RadTextBox)e.Item.FindControl("txtInspectionTypeAdd")).Text
                        , (((RadComboBox)e.Item.FindControl("ddlQuestionTypeAdd")).SelectedValue)
                    );
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteInspection(new Guid(((RadLabel)e.Item.FindControl("lblInspectionId")).Text));
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("COPY"))
            {
                string docname = "";
                docname = "Audit";

                ViewState["InspectionId"] = ((RadLabel)e.Item.FindControl("lblInspectionId")).Text;

                if (ViewState["InspectionId"] != null)
                {
                    String script = String.Format("javascript:fnReloadList('codehelp1');");
                    RadWindowManager1.RadConfirm("This will create a new '" + docname + "' with all the chapters, topics and checklists from the selected " + docname + " and it will be effective from today.\n" + "Do you want to continue.?", "btnconfirm", 320, 150, null, "Confirm");

                    //ucConfirm.HeaderMessage = "Please Confirm";
                    //ucConfirm.Text = "This will create a new '" + docname + "' with all the chapters, topics and checklists from the selected " + docname + " and it will be effective from today.\n" + "Do you want to continue.?";
                    //ucConfirm.Visible = true;
                }
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {

                if (!IsValidInspection(
                         ((RadLabel)e.Item.FindControl("lblInspectionTypeIdEdit")).Text
                        , ((UserControlHard)e.Item.FindControl("ucInspectionCategoryEdit")).SelectedHard
                        , ((RadTextBox)e.Item.FindControl("txtInspectionNameEdit")).Text
                        , ((RadTextBox)e.Item.FindControl("txtInspectionShortCodeEdit")).Text
                        , ((UserControlDate)e.Item.FindControl("ucEffectiveDateEdit")).Text
                        , ((UserControlHard)e.Item.FindControl("ucExternalAuditTypeEdit")).SelectedHard
                        , (((RadCheckBox)e.Item.FindControl("chkOfficeAuditYNEdit")).Checked == true) ? "1" : "0"
                        , ((UserControlCompany)e.Item.FindControl("ucCompanyEdit")).SelectedCompany
                        , ((UserControlMaskNumber)e.Item.FindControl("txtFrequencyEdit")).Text
                        , (((RadComboBox)e.Item.FindControl("ddlQuestionTypeEdit")).SelectedValue)

                        ))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateInspection(
                        new Guid(((RadLabel)e.Item.FindControl("lblInspectionIdEdit")).Text)
                        , ((RadLabel)e.Item.FindControl("lblInspectionTypeIdEdit")).Text
                        , ((UserControlHard)e.Item.FindControl("ucInspectionCategoryEdit")).SelectedHard
                        , ((RadTextBox)e.Item.FindControl("txtInspectionNameEdit")).Text
                        , ((RadTextBox)e.Item.FindControl("txtInspectionShortCodeEdit")).Text
                        , (((RadCheckBox)e.Item.FindControl("chkActiveYNEdit")).Checked == true) ? 1 : 0
                        , ((UserControlDate)e.Item.FindControl("ucEffectiveDateEdit")).Text
                        , ((UserControlHard)e.Item.FindControl("ucExternalAuditTypeEdit")).SelectedHard
                        , (((RadCheckBox)e.Item.FindControl("chkOfficeAuditYNEdit")).Checked == true) ? 1 : 0
                        , ((UserControlCompany)e.Item.FindControl("ucCompanyEdit")).SelectedCompany
                        , ((UserControlMaskNumber)e.Item.FindControl("txtFrequencyEdit")).Text
                        , (((RadCheckBox)e.Item.FindControl("chkAddToScheduleYNEdit")).Checked == true) ? 1 : 0
                        , ((UserControlMaskNumber)e.Item.FindControl("txtWindowBeforeEdit")).Text
                        , ((UserControlMaskNumber)e.Item.FindControl("txtWindowAfterEdit")).Text
                        , ((RadTextBox)e.Item.FindControl("txtInspectionTypeEdit")).Text
                        , (((RadComboBox)e.Item.FindControl("ddlQuestionTypeEdit")).SelectedValue)

                        );

                Rebind();
            }
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvInspection_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvInspection.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvInspection_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            UserControlHard ucHard = (UserControlHard)e.Item.FindControl("ucInspectionCategoryEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ucHard != null) ucHard.SelectedHard = drv["FLDINSPECTIONCATEGORYID"].ToString();

            UserControlCompany ucCompanyEdit = (UserControlCompany)e.Item.FindControl("ucCompanyEdit");
            if (ucCompanyEdit != null)
            {
                if (drv["FLDCOMPANYID"] != null && drv["FLDCOMPANYID"].ToString() != "")
                    ucCompanyEdit.SelectedCompany = drv["FLDCOMPANYID"].ToString();
                else
                    ucCompanyEdit.SelectedCompany = ViewState["COMPANYID"].ToString();
            }

            UserControlHard ucExternalAuditType = (UserControlHard)e.Item.FindControl("ucExternalAuditTypeEdit");
            if (ucExternalAuditType != null) ucExternalAuditType.SelectedHard = drv["FLDEXTERNALAUDITTYPE"].ToString();

            if (drv["FLDINSPECTIONCATEGORYID"] != null && drv["FLDINSPECTIONCATEGORYID"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 144, "EXT"))
            {
                if (ucExternalAuditType != null)
                {
                    //ucExternalAuditType.CssClass = "dropdown_mandatory";
                    ucExternalAuditType.Enabled = true;
                }
            }
            else
            {
                if (ucExternalAuditType != null)
                {
                    //ucExternalAuditType.CssClass = "input";
                    ucExternalAuditType.Enabled = false;
                    ucExternalAuditType.SelectedHard = "";
                }
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            LinkButton cmdMap = (LinkButton)e.Item.FindControl("cmdMap");
            if (cmdMap != null)
            {
                if (drv["FLDOFFICEYN"] != null && drv["FLDOFFICEYN"].ToString() == "1")
                    cmdMap.Visible = true;
                else
                    cmdMap.Visible = false;
                if (!SessionUtil.CanAccess(this.ViewState, cmdMap.CommandName)) cmdMap.Visible = false;
                cmdMap.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionCompanyMapping.aspx?INSPECTIONID=" + drv["FLDINSPECTIONID"].ToString() + "');return false;");
            }
            LinkButton lnkConfig = (LinkButton)e.Item.FindControl("lnkConfig");
            if (lnkConfig != null)
            {
                lnkConfig.Attributes.Add("onclick", "javascript:openNewWindow('Configuration', 'Map Check Items', '" + Session["sitepath"] + "/Inspection/InspectionDeficiencyCheckitemAdd.aspx?INSPECTIONID=" + drv["FLDINSPECTIONID"].ToString() + "');return false;");
            }

            LinkButton cmdQuestion = (LinkButton)e.Item.FindControl("cmdQuestion");
            RadLabel lnkInspectionName = (RadLabel)e.Item.FindControl("lnkInspectionName");
            if (cmdQuestion != null)
            {
                SessionUtil.CanAccess(this.ViewState, cmdQuestion.CommandName);
                cmdQuestion.Attributes.Add("onclick", "openNewWindow('Questions', '" + lnkInspectionName.Text + " - Question', '" + Session["sitepath"] + "/Inspection/InspectionQuestions.aspx?INSPECTIONID=" + drv["FLDINSPECTIONID"].ToString() + "');return false;");
            }

            RadComboBox gre = (RadComboBox)e.Item.FindControl("ddlQuestionTypeEdit");
            if (gre != null)
            {
                DataSet ds = new DataSet();
                ds = PhoenixInspection.QuestionType();
                gre.DataSource = ds;
                gre.DataTextField = "FLDQUICKNAME";
                gre.DataValueField = "FLDQUICKTYPECODE";
                gre.DataBind();
                gre.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                gre.SelectedValue = DataBinder.Eval(e.Item.DataItem, "FLDQUICKTYPECODE").ToString();
            }
            if(eb != null)
            {
                eb.Attributes.Add("OnClick", "javascript:openNewWindow('IndpecDetail', 'Edit Inspection', '" + Session["sitepath"] + "/Inspection/InspectionAuditDetailEdit.aspx?INSPECTIONID=" + drv["FLDINSPECTIONID"].ToString() + "');return false;");
            }
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            RadTextBox txtInspectionType = (RadTextBox)e.Item.FindControl("txtInspectionType");
            if (txtInspectionType != null)
                txtInspectionType.Text = "Audit";

            UserControlCompany ucCompanyAdd = (UserControlCompany)e.Item.FindControl("ucCompanyAdd");
            if (ucCompanyAdd != null)
                ucCompanyAdd.SelectedCompany = ViewState["COMPANYID"].ToString();

            RadComboBox gra = (RadComboBox)e.Item.FindControl("ddlQuestionTypeAdd");
            if (gra != null)
            {
                DataSet ds = new DataSet();
                ds = PhoenixInspection.QuestionType();
                gra.DataSource = ds;
                gra.DataTextField = "FLDQUICKNAME";
                gra.DataValueField = "FLDQUICKTYPECODE";
                gra.DataBind();
               // gra.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            }
        }
    }

    protected void gvInspection_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        Rebind();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = null;
        gvInspection.Rebind();
    }
}
