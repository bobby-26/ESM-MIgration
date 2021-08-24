using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class CrewOffshoreAppraisalTrainingNeeds : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreAppraisalTrainingNeeds.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvOffshoreTraining')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuOffshoreTraining.AccessRights = this.ViewState;
            MenuOffshoreTraining.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                confirm.Attributes.Add("style", "display:none");
                ViewState["PAGENUMBER"] = 1;
                ViewState["appraisalid"] = "";
                ViewState["employeeid"] = "";
                ViewState["vesselid"] = "";
                ViewState["rating"] = "";
                ViewState["CompetenceId"] = "";
                ViewState["QuestionId"] = "";
                ViewState["categoryId"] = "";
                ucConfirmDelete.Visible = false;

                if (Request.QueryString["appraisalid"] != null && Request.QueryString["appraisalid"].ToString() != "")
                {
                    ViewState["appraisalid"] = Request.QueryString["appraisalid"].ToString();
                }
                if (Request.QueryString["employeeid"] != null && Request.QueryString["employeeid"].ToString() != "")
                {
                    ViewState["employeeid"] = Request.QueryString["employeeid"].ToString();
                }
                if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"].ToString() != "")
                {
                    ViewState["vesselid"] = Request.QueryString["vesselid"].ToString();
                }
                if (Request.QueryString["rating"] != null && Request.QueryString["rating"].ToString() != "")
                {
                    ViewState["rating"] = Request.QueryString["rating"].ToString();
                }
                if (Request.QueryString["CompetenceId"] != null && Request.QueryString["CompetenceId"].ToString() != "")
                {
                    ViewState["CompetenceId"] = Request.QueryString["CompetenceId"].ToString();
                }
                if (Request.QueryString["QuestionId"] != null && Request.QueryString["QuestionId"].ToString() != "")
                {
                    ViewState["QuestionId"] = Request.QueryString["QuestionId"].ToString();
                }
                if (Request.QueryString["categoryId"] != null && Request.QueryString["categoryId"].ToString() != "")
                {
                    ViewState["categoryId"] = Request.QueryString["categoryId"].ToString();
                }

                gvOffshoreTraining.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
              
            }
           // BindData();
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindCategory(DropDownList ddl)
    {
        ddl.Items.Clear();
        DataSet ds = PhoenixCrewOffshoreTrainingCategory.ListTrainingCategory(null);
        ddl.DataTextField = "FLDCATEGORYNAME";
        ddl.DataValueField = "FLDCATEGORYID";
        ddl.DataSource = ds;
        ddl.Items.Insert(0, new ListItem("--Select--", ""));
        ddl.DataBind();
    }

    protected void BindSubCategory(RadComboBox ddl, string categoryid)
    {
        ddl.Items.Clear();
        DataTable dt = PhoenixCrewOffshoreTrainingSubCategory.ListTrainingSubCategory(General.GetNullableInteger(ViewState["categoryId"].ToString()));
        ddl.DataTextField = "FLDNAME";
        ddl.DataValueField = "FLDSUBCATEGORYID";
        ddl.DataSource = dt;
        ddl.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
        ddl.DataBind();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROW", "FLDSUBCATEGORYNAME", "FLDTRAININGNEED", "FLDLEVELOFIMPROVEMENTNAME", "FLDTYPEOFTRAININGNAME", "FLDREMARKS" };
        string[] alCaptions = { "No", "SubCategory", "Identified Training Need", "Level of Improvement", "Type of Training", "Remarks" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixCrewOffshoreTrainingNeeds.SearchTrainingNeeds(int.Parse(ViewState["employeeid"].ToString()),
                            General.GetNullableGuid(ViewState["appraisalid"].ToString()),
                            sortexpression, sortdirection,
                            1,
                            iRowCount,
                            ref iRowCount,
                            ref iTotalPageCount, General.GetNullableGuid(ViewState["CompetenceId"].ToString()),null, General.GetNullableInteger(ViewState["categoryId"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=OffshoreTrainingNeeds.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Training Needs</h3></td>");
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
        foreach (DataRow dr in dt.Rows)
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

    protected void MenuOffshoreTraining_TabStripCommand(object sender, EventArgs e)
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
        string[] alColumns = { "FLDROW", "FLDSUBCATEGORYNAME", "FLDTRAININGNEED", "FLDLEVELOFIMPROVEMENTNAME", "FLDTYPEOFTRAININGNAME", "FLDREMARKS" };
        string[] alCaptions = { "No", "SubCategory", "Identified Training Need", "Level of Improvement", "Type of Training", "Remarks" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixCrewOffshoreTrainingNeeds.SearchTrainingNeeds(int.Parse(ViewState["employeeid"].ToString()),
                General.GetNullableGuid(ViewState["appraisalid"].ToString()),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvOffshoreTraining.PageSize,
                ref iRowCount,
                ref iTotalPageCount, General.GetNullableGuid(ViewState["CompetenceId"].ToString()), null, General.GetNullableInteger(ViewState["categoryId"].ToString()));

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvOffshoreTraining", "Training Needs", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvOffshoreTraining.DataSource = ds;
       
        }
        else
        {
            gvOffshoreTraining.DataSource = ds;
        }
        gvOffshoreTraining.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        
    }

    protected void gvOffshoreTraining_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;
            BindData();
            ((DropDownList)_gridView.Rows[de.NewEditIndex].FindControl("ddlSubCategoryEdit")).Focus();
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOffshoreTraining_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

   
   
    private void CallScript()
    {
        String script = "javascript:fnReloadList('codehelp1', 'ifMoreInfo', true);";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }
    protected void gvOffshoreTraining_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            BindData();
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOffshoreTraining_OnRowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
     
    }
    protected void btnConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            

                //int nCurrentRow = int.Parse(ViewState["CURRENTROW"].ToString());
                Guid trainingneedid= new Guid(ViewState["TRAININGID"].ToString());
             
               // Guid trainingneedid = new Guid(gvOffshoreTraining.MasterTableView.Item.DataKeyNames[.DataKeys[nCurrentRow].Value.ToString());

                PhoenixCrewOffshoreTrainingNeeds.DeleteTrainingNeed(trainingneedid);
                BindData();
                gvOffshoreTraining.Rebind();
                CallScript();

                ucStatus.Text = "Training Need is deleted successfully.";
                BindData();
                gvOffshoreTraining.Rebind();
               
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidData(string Category, string SubCategory, string trainingneed, string improvementlevel, string typeoftraining,string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        //if (General.GetNullableInteger(Category) == null)
        //    ucError.ErrorMessage = "Category is required.";

        if (General.GetNullableInteger(SubCategory) == null)
            ucError.ErrorMessage = "SubCategory is required.";

        if (General.GetNullableString(trainingneed) == null)
            ucError.ErrorMessage = "Identified training need is required.";

        if ((General.GetNullableInteger(ViewState["rating"].ToString()) >= 5) && General.GetNullableInteger(improvementlevel) == null)
            ucError.ErrorMessage = "Level of improvement is required.";

        if ((General.GetNullableInteger(ViewState["rating"].ToString()) >= 5) && General.GetNullableInteger(typeoftraining) == null)
            ucError.ErrorMessage = "Type of Training is required";

        if ((General.GetNullableInteger(ViewState["rating"].ToString()) >= 5) && string.IsNullOrEmpty(remarks))
            ucError.ErrorMessage = "Remarks is required";

        return (!ucError.IsError);
    }



    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void ddlSubCategoryEdit_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        GridViewRow gvrow = (GridViewRow)ddl.Parent.Parent;
       // DropDownList ddlCategoryEdit = (DropDownList)gvrow.FindControl("ddlCategoryEdit");
        //CheckBoxList chk = (CheckBoxList)gvrow.FindControl("ucDocumentCourseEdit");       
        //    if (General.GetNullableInteger(ddl.SelectedValue).HasValue)
        //        BindCource(chk, ViewState["categoryId"].ToString(), ddl.SelectedValue);
        
    }
    protected void ddlSubCategoryAdd_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadComboBox  ddl = (RadComboBox)sender;
        GridFooterItem gvrow = (GridFooterItem)ddl.Parent.Parent;
        //DropDownList ddlCategoryAdd = (DropDownList)gvrow.FindControl("ddlCategoryAdd");
        //CheckBoxList chk = (CheckBoxList)gvrow.FindControl("ucDocumentCourseAdd");       
        //    if (General.GetNullableInteger(ddl.SelectedValue).HasValue)
        //        BindCource(chk, ViewState["categoryId"].ToString(), ddl.SelectedValue);
        
    }

    private void BindCource(CheckBoxList cbl, string category, string subcategory)
    {
        cbl.DataSource = PhoenixRegistersDocumentCourse.ListDocumentCourseFilter(null, General.GetNullableInteger(category), General.GetNullableInteger(subcategory));
        cbl.DataTextField = "FLDDOCUMENTNAME";
        cbl.DataValueField = "FLDDOCUMENTID";
        cbl.DataBind();
    }

    protected void gvOffshoreTraining_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOffshoreTraining.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvOffshoreTraining_ItemDataBound(object sender, GridItemEventArgs e)
    {
       

        if (e.Item is GridDataItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            RadComboBox ddlSubCategoryEdit = (RadComboBox)e.Item.FindControl("ddlSubCategoryEdit");
            if (ddlSubCategoryEdit != null)
            {
                BindSubCategory(ddlSubCategoryEdit, ViewState["categoryId"].ToString());
                if (dr["FLDSUBCATEGORY"] != null && dr["FLDSUBCATEGORY"].ToString() != "")
                {
                    if (ddlSubCategoryEdit.Items.FindItemByValue(dr["FLDSUBCATEGORY"].ToString()) != null)
                        ddlSubCategoryEdit.SelectedValue = dr["FLDSUBCATEGORY"].ToString();
                }
            }

            UserControlQuick ucImprovementEdit = (UserControlQuick)e.Item.FindControl("ucImprovementEdit");
            if (ucImprovementEdit != null)
            {
                ucImprovementEdit.bind();
                ucImprovementEdit.SelectedQuick = dr["FLDLEVELOFIMPROVEMENT"].ToString();
            }

            UserControlQuick ucTypeofTrainingEdit = (UserControlQuick)e.Item.FindControl("ucTypeofTrainingEdit");
            if (ucTypeofTrainingEdit != null)
            {
                ucTypeofTrainingEdit.bind();
                ucTypeofTrainingEdit.SelectedQuick = dr["FLDTYPEOFTRAINING"].ToString();
            }
            RadTextBox txRemarksEdit = (RadTextBox)e.Item.FindControl("txRemarksEdit");

            if ((General.GetNullableInteger(ViewState["rating"].ToString())) >= 5)
            {
                if (ucImprovementEdit != null)
                {
                    ucImprovementEdit.Visible = true;
                    ucImprovementEdit.CssClass = "input_mandatory";
                }
                if (ucTypeofTrainingEdit != null)
                {
                    ucTypeofTrainingEdit.Visible = true;
                    ucTypeofTrainingEdit.CssClass = "input_mandatory";
                }
                if (txRemarksEdit != null)
                    txRemarksEdit.CssClass = "input_mandatory";
            }

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
                //db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }

            CheckBoxList ucDocumentCourseEdit = (CheckBoxList)e.Item.FindControl("ucDocumentCourseEdit");
            if (ucDocumentCourseEdit != null)
            {
                BindCource(ucDocumentCourseEdit, ViewState["categoryId"].ToString(), ddlSubCategoryEdit.SelectedValue);
                CheckBoxList chk = (CheckBoxList)e.Item.FindControl("ucDocumentCourseEdit");
                General.BindCheckBoxList(chk, dr["FLDDOCUMENTID"].ToString());
            }

        }
        if (e.Item is GridFooterItem)
        {

            RadComboBox ddlSubCategoryAdd = (RadComboBox)e.Item.FindControl("ddlSubCategoryAdd");
            if (ddlSubCategoryAdd != null)
            {
                BindSubCategory(ddlSubCategoryAdd, ViewState["categoryId"].ToString());
            }

            UserControlQuick ucImprovementAdd = (UserControlQuick)e.Item.FindControl("ucImprovementAdd");
            if (ucImprovementAdd != null)
                ucImprovementAdd.bind();

            UserControlQuick ucTypeofTrainingAdd = (UserControlQuick)e.Item.FindControl("ucTypeofTrainingAdd");
            if (ucTypeofTrainingAdd != null)
                ucTypeofTrainingAdd.bind();

            RadTextBox txtRemarksAdd = (RadTextBox)e.Item.FindControl("txtRemarksAdd");

            if ((General.GetNullableInteger(ViewState["rating"].ToString())) >= 5)
            {
                if (ucImprovementAdd != null)
                {
                    ucImprovementAdd.Visible = true;
                    ucImprovementAdd.CssClass = "input_mandatory";
                }
                if (ucTypeofTrainingAdd != null)
                {
                    ucTypeofTrainingAdd.Visible = true;
                    ucTypeofTrainingAdd.CssClass = "input_mandatory";
                }
                if (txtRemarksAdd != null)
                    txtRemarksAdd.CssClass = "input_mandatory";
            }

            LinkButton ab = (LinkButton)e.Item.FindControl("cmdAdd");
            if (ab != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName))
                    ab.Visible = false;
            }
        }
    }

    protected void gvOffshoreTraining_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
           
           // int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                
                if (!IsValidData(ViewState["categoryId"].ToString()
                    , ((RadComboBox)e.Item.FindControl("ddlSubCategoryAdd")).SelectedValue
                    , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtTrainingNeedAdd")).Text)
                    , ((UserControlQuick)e.Item.FindControl("ucImprovementAdd")).SelectedQuick
                    , ((UserControlQuick)e.Item.FindControl("ucTypeofTrainingAdd")).SelectedQuick
                    , ((RadTextBox)e.Item.FindControl("txtRemarksAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewOffshoreTrainingNeeds.InsertAppraisalTrainingNeeds(int.Parse(ViewState["vesselid"].ToString()),
                    int.Parse(ViewState["employeeid"].ToString()), new Guid(ViewState["appraisalid"].ToString()),
                    General.GetNullableInteger(ViewState["categoryId"].ToString()),
                    General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlSubCategoryAdd")).SelectedValue),
                    General.GetNullableString(((RadTextBox)e.Item.FindControl("txtTrainingNeedAdd")).Text),
                    General.GetNullableInteger(((UserControlQuick)e.Item.FindControl("ucImprovementAdd")).SelectedQuick),
                   null,
                   null, General.GetNullableInteger(ViewState["rating"].ToString()),
                    General.GetNullableInteger(ViewState["categoryId"].ToString()),
                    General.GetNullableInteger(((UserControlQuick)e.Item.FindControl("ucTypeofTrainingAdd")).SelectedQuick),
                    ((RadTextBox)e.Item.FindControl("txtRemarksAdd")).Text);

                BindData();
                ((RadComboBox)e.Item.FindControl("ddlSubCategoryAdd")).Focus();

                CallScript();
                gvOffshoreTraining.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;

                Guid trainingneedid = new Guid(eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDTRAININGNEEDID"].ToString());
               
                //ViewState["CURRENTROW"] = nCurrentRow;
                ViewState["TRAININGID"] = trainingneedid;
                //ucConfirmDelete.Visible = true;
                //ucConfirmDelete.Text = "When the Training Need is deleted then course request will also be deleted. Do you want to continue..?";
                //return;
                RadWindowManager1.RadConfirm("You will not be able to make any changes. Are you sure you want to save the record?", "confirm", 320, 150, null, "Confirm");


            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
               

                if (!IsValidData(ViewState["categoryId"].ToString()
                    , ((RadLabel)e.Item.FindControl("lblSubCategoryIdEdit")).Text
                    , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtTrainingNeedEdit")).Text)
                    , ((UserControlQuick)e.Item.FindControl("ucImprovementEdit")).SelectedQuick
                    , ((UserControlQuick)e.Item.FindControl("ucTypeofTrainingEdit")).SelectedQuick
                    , ((RadTextBox)e.Item.FindControl("txRemarksEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                GridEditableItem eeditedItem = e.Item as GridEditableItem;

                Guid trainingneedid = new Guid(eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDTRAININGNEEDID"].ToString());


                PhoenixCrewOffshoreTrainingNeeds.UpdateAppraisalTrainingNeeds(trainingneedid,
                    General.GetNullableInteger(ViewState["categoryId"].ToString()),
                    General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblSubCategoryIdEdit")).Text),
                    General.GetNullableString(((RadTextBox)e.Item.FindControl("txtTrainingNeedEdit")).Text),
                    General.GetNullableInteger(((UserControlQuick)e.Item.FindControl("ucImprovementEdit")).SelectedQuick),
                    null,
                   null, General.GetNullableInteger(ViewState["rating"].ToString()),
                    General.GetNullableInteger(ViewState["QuestionId"].ToString()),
                    General.GetNullableInteger(((UserControlQuick)e.Item.FindControl("ucTypeofTrainingEdit")).SelectedQuick),
                    ((RadTextBox)e.Item.FindControl("txRemarksEdit")).Text);

               
                BindData();
                gvOffshoreTraining.Rebind();
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
}
