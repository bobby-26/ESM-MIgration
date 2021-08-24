using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionInspectionList : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionInspectionList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvInspection')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
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
            if (Request.QueryString["type"] != null && Request.QueryString["type"].ToString() != string.Empty)
            {
                ViewState["TYPE"] = Request.QueryString["type"].ToString();
                if (ViewState["TYPE"] != null && ViewState["TYPE"].ToString() != string.Empty && ViewState["TYPE"].ToString() == "AUDIT")
                    lblTitle.Text = "Audit";
                else if (ViewState["TYPE"] != null && ViewState["TYPE"].ToString() != string.Empty && ViewState["TYPE"].ToString() == "INSPECTION")
                    lblTitle.Text = "Vetting";
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

        string[] alColumns = { "FLDCOMPANYNAME", "FLDINSPECTIONCATEGORYNAME", "FLDSHORTCODE", "FLDINSPECTIONNAME", "FLDACTIVEYN", "FLDFREQUENCYINMONTHS" };
        string[] alCaptions = { "Company", "Category", "Short Code", "Name", "Active Y/N", "Frequency (in months)" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if (ViewState["TYPE"] != null && ViewState["TYPE"].ToString() != string.Empty && ViewState["TYPE"].ToString() == "AUDIT")
        {
            typeid = 726;
            docname = "Audit/Inspection";
        }
        else if (ViewState["TYPE"] != null && ViewState["TYPE"].ToString() != string.Empty && ViewState["TYPE"].ToString() == "INSPECTION")
        {
            typeid = 725;
            docname = "Vetting";
        }

        DataSet ds = PhoenixInspection.InspectionSearch(
           General.GetNullableInteger(typeid.ToString())
         , General.GetNullableInteger(ucInspectionCategory.SelectedHard)
         , null
         , sortexpression, sortdirection
         , 1
         , iRowCount
         , ref iRowCount
         , ref iTotalPageCount
         , null
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int typeid = 0;
        string docname = "";

        string[] alColumns = { "FLDCOMPANYNAME", "FLDINSPECTIONCATEGORYNAME", "FLDSHORTCODE", "FLDINSPECTIONNAME", "FLDACTIVEYN", "FLDFREQUENCYINMONTHS" };
        string[] alCaptions = { "Company", "Category", "Short Code", "Name", "Active Y/N", "Frequency (in months)" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["TYPE"] != null && ViewState["TYPE"].ToString() != string.Empty && ViewState["TYPE"].ToString() == "AUDIT")
        {
            typeid = 726;
            docname = "Audit/Inspection";
        }
        else if (ViewState["TYPE"] != null && ViewState["TYPE"].ToString() != string.Empty && ViewState["TYPE"].ToString() == "INSPECTION")
        {
            typeid = 725;
            docname = "Vetting";
        }
        DataSet ds = PhoenixInspection.InspectionSearch(
         General.GetNullableInteger(typeid.ToString())
         , General.GetNullableInteger(ucInspectionCategory.SelectedHard)
         , null
         , sortexpression, sortdirection
         , Int32.Parse(ViewState["PAGENUMBER"].ToString())
         , gvInspection.PageSize
         , ref iRowCount
         , ref iTotalPageCount
         , null
         , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));


        General.SetPrintOptions("gvInspection", docname, alCaptions, alColumns, ds);

        gvInspection.DataSource = ds;
        gvInspection.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void Rebind()
    {
        gvInspection.SelectedIndexes.Clear();
        gvInspection.EditIndexes.Clear();
        gvInspection.DataSource = null;
        gvInspection.Rebind();
    }

    private void InsertInspection(string inspectiontypeid, string inspectiontypecategoryid, string inspectionname, string shortcode,
        int activeyn, string effectivedate, string frequency, string companyid, string firstreminder, string secondreminder,
        string Type, string QuestionType)
    {

        PhoenixInspection.InsertInspection(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , Int16.Parse(inspectiontypeid)
            , Int16.Parse(inspectiontypecategoryid)
            , inspectionname
            , shortcode
            , activeyn
            , null
            , null
            , null
            , General.GetNullableInteger(companyid)
            , General.GetNullableInteger(frequency)
            , General.GetNullableInteger(firstreminder)
            , General.GetNullableInteger(secondreminder)
            , General.GetNullableString(Type)
            , General.GetNullableInteger(QuestionType)
            );
    }

    private void UpdateInspection(Guid inspectionid, string inspectiontypeid, string inspectiontypecategoryid, string inspectionname,
        string shortcode, int activeyn, string effectivedate, string frequency, string companyid, string firstreminder, string secondreminder,
        Guid? activeinspectionid, string Type, string QuestionType)
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
            , null
            , null
            , General.GetNullableInteger(companyid)
            , General.GetNullableInteger(frequency)
            , General.GetNullableInteger(firstreminder)
            , General.GetNullableInteger(secondreminder)
            , activeinspectionid
            , General.GetNullableString(Type)
            , General.GetNullableInteger(QuestionType)
            );
        ucStatus.Text = "Information updated";
    }

    private bool IsValidInspection(string inspectiontypeid, string inspectiontypecategoryid, string inspectionname, string shortcode,
        string effectivedate, string frequency, string companyid, string QuestionType)
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

        //if (General.GetNullableDateTime(effectivedate) != null && General.GetNullableDateTime(effectivedate) < DateTime.Today)
        //    ucError.ErrorMessage = "Effective date should not be past date.";

        return (!ucError.IsError);
    }

    private bool IsValidInspectionupdate(string inspectiontypeid, string inspectiontypecategoryid, string inspectionname, string shortcode,
       string effectivedate, string frequency, string companyid, string activeinispectionid, string QuestionType)
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
        if (General.GetNullableGuid(activeinispectionid) == null)
            ucError.ErrorMessage = "Active Inspection is required.";
        if (General.GetNullableInteger(QuestionType) == null)
            ucError.ErrorMessage = "QuestionType is required.";

        //if (General.GetNullableDateTime(effectivedate) != null && General.GetNullableDateTime(effectivedate) < DateTime.Today)
        //    ucError.ErrorMessage = "Effective date should not be past date.";

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


    public void ucInspectionCategory_Changed(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }

    protected void gvInspection_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                int typeid = 0;
                if (ViewState["TYPE"] != null && ViewState["TYPE"].ToString() != string.Empty && ViewState["TYPE"].ToString() == "AUDIT")
                    typeid = 726;
                else if (ViewState["TYPE"] != null && ViewState["TYPE"].ToString() != string.Empty && ViewState["TYPE"].ToString() == "INSPECTION")
                    typeid = 725;

                if (!IsValidInspection(typeid.ToString()
                        , ((UserControlHard)e.Item.FindControl("ucInspectionCategoryAdd")).SelectedHard
                        , ((RadTextBox)e.Item.FindControl("txtInspectionNameAdd")).Text
                        , ((RadTextBox)e.Item.FindControl("txtInspectionShortCodeAdd")).Text
                        , ((UserControlDate)e.Item.FindControl("ucEffectiveDateAdd")).Text
                        , ((UserControlMaskNumber)e.Item.FindControl("txtFrequencyAdd")).Text
                        , ((UserControlCompany)e.Item.FindControl("ucCompanyAdd")).SelectedCompany
                        , ((RadComboBox)e.Item.FindControl("ddlQuestionTypeAdd")).SelectedValue
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
                        , ((UserControlMaskNumber)e.Item.FindControl("txtFrequencyAdd")).Text
                        , ((UserControlCompany)e.Item.FindControl("ucCompanyAdd")).SelectedCompany
                        , ((UserControlMaskNumber)e.Item.FindControl("txtFirstReminderAdd")).Text
                        , ((UserControlMaskNumber)e.Item.FindControl("txtSecondReminderAdd")).Text
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

                if (ViewState["TYPE"] != null && ViewState["TYPE"].ToString() != string.Empty && ViewState["TYPE"].ToString() == "AUDIT")
                    docname = "Audit";
                else if (ViewState["TYPE"] != null && ViewState["TYPE"].ToString() != string.Empty && ViewState["TYPE"].ToString() == "INSPECTION")
                    docname = "Inspection";

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
                if (!IsValidInspectionupdate(
                       ((RadLabel)e.Item.FindControl("lblInspectionTypeIdEdit")).Text
                      , ((UserControlHard)e.Item.FindControl("ucInspectionCategoryEdit")).SelectedHard
                      , ((RadTextBox)e.Item.FindControl("txtInspectionNameEdit")).Text
                      , ((RadTextBox)e.Item.FindControl("txtInspectionShortCodeEdit")).Text
                      , ((UserControlDate)e.Item.FindControl("ucEffectiveDateEdit")).Text
                      , ((UserControlMaskNumber)e.Item.FindControl("txtFrequencyEdit")).Text
                      , ((UserControlCompany)e.Item.FindControl("ucCompanyEdit")).SelectedCompany
                      , ((RadComboBox)e.Item.FindControl("ddlinspectionidEdit")).SelectedValue
                      , ((RadComboBox)e.Item.FindControl("ddlQuestionTypeEdit")).SelectedValue))
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
                        , ((UserControlMaskNumber)e.Item.FindControl("txtFrequencyEdit")).Text
                        , ((UserControlCompany)e.Item.FindControl("ucCompanyEdit")).SelectedCompany
                        , ((UserControlMaskNumber)e.Item.FindControl("txtFirstReminderEdit")).Text
                        , ((UserControlMaskNumber)e.Item.FindControl("txtSecondReminderEdit")).Text
                        , General.GetNullableGuid(((RadComboBox)e.Item.FindControl("ddlinspectionidEdit")).SelectedValue)
                        , ((RadTextBox)e.Item.FindControl("txtInspectionTypeEdit")).Text
                        , (((RadComboBox)e.Item.FindControl("ddlQuestionTypeEdit")).SelectedValue)

                        );
                Rebind();
            }
            else if (e.CommandName == "Page")
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

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            RadComboBox ddlinspectionidEdit = (RadComboBox)e.Item.FindControl("ddlinspectionidEdit");

            string type = PhoenixCommonRegisters.GetHardCode(1, 148, "INS");

            if (ddlinspectionidEdit != null)
            {
                DataSet ds = PhoenixInspection.ListInspectionByCompany(General.GetNullableInteger(type)
                                            , null
                                            , null
                                            , 1
                                            , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
                ddlinspectionidEdit.DataSource = ds.Tables[0];
                ddlinspectionidEdit.DataTextField = "FLDSHORTCODE";
                ddlinspectionidEdit.DataValueField = "FLDINSPECTIONID";
                ddlinspectionidEdit.DataBind();
                //  ddlinspectionidEdit.Items.Insert(0, new ListItem("--Select--", "Dummy"));

                ddlinspectionidEdit.SelectedValue = drv["FLDACTIVEINSPECTIONID"].ToString();
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
            {
                if (ViewState["TYPE"] != null && ViewState["TYPE"].ToString() != string.Empty && ViewState["TYPE"].ToString() == "AUDIT")
                    txtInspectionType.Text = "Audit";
                else if (ViewState["TYPE"] != null && ViewState["TYPE"].ToString() != string.Empty && ViewState["TYPE"].ToString() == "INSPECTION")
                    txtInspectionType.Text = "Inspection";
            }

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
                //gra.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
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
}
