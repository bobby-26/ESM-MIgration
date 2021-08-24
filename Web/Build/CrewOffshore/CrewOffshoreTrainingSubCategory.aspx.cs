using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class CrewOffshoreTrainingSubCategory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../CrewOffshore/CrewOffshoreTrainingSubCategory.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvOffshoreSubCategory')", "Print Grid", "icon_print.png", "PRINT");
            MenuOffshoreSubCategory.AccessRights = this.ViewState;
            MenuOffshoreSubCategory.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                BindCategory();
            }
            
            //BindData();
            //SetPageNavigator();
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

        string[] alColumns = { "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME" };
        string[] alCaptions = { "Category", "SubCategory" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixCrewOffshoreTrainingSubCategory.SearchTrainingSubCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            null, sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                iRowCount,
                ref iRowCount,
                ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=OffshoreCompetenceSubCategory.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Training SubCategory</h3></td>");
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

    protected void MenuOffshoreSubCategory_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
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

    protected void BindCategory()
    {
        DataSet ds = PhoenixCrewOffshoreTrainingCategory.ListTrainingCategory(null);
        ddlCompetenceCategory.DataTextField = "FLDCATEGORYNAME";
        ddlCompetenceCategory.DataValueField = "FLDCATEGORYID";
        ddlCompetenceCategory.DataSource = ds;
        ddlCompetenceCategory.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
        ddlCompetenceCategory.DataBind();

    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME" };
        string[] alCaptions = { "Category", "SubCategory" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixCrewOffshoreTrainingSubCategory.SearchTrainingSubCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            null, sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount,
                General.GetNullableInteger(ddlCompetenceCategory.SelectedValue));

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvOffshoreSubCategory", "Competence SubCategory", alCaptions, alColumns, ds);
        gvOffshoreSubCategory.DataSource = ds;
        //gvOffshoreSubCategory.DataBind();

        

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
       // SetPageNavigator();
    }

    protected void gvOffshoreSubCategory_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;
            BindData();
            ((RadComboBox)_gridView.Rows[de.NewEditIndex].FindControl("ddlCategoryEdit")).Focus();
          //  SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOffshoreSubCategory_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

  

    protected void gvOffshoreSubCategory_OnRowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
       // SetPageNavigator();
    }
    private bool IsValidData(string Category, string SubCategory)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(Category)== null)
            ucError.ErrorMessage = "Category is required.";
        if(string.IsNullOrEmpty(SubCategory))
            ucError.ErrorMessage = "SubCategory is required.";

        return (!ucError.IsError);
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        //gvOffshoreSubCategory.SelectedIndex = -1;
        //gvOffshoreSubCategory.EditIndex = -1;
        ViewState["PAGENUMBER"] = 1;
        BindData();
       // SetPageNavigator();
    }

    //protected void cmdGo_Click(object sender, EventArgs e)
    //{
    //    int result;
    //    gvOffshoreSubCategory.SelectedIndex = -1;
    //    gvOffshoreSubCategory.EditIndex = -1;
    //    if (Int32.TryParse(txtnopage.Text, out result))
    //    {
    //        ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

    //        if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];


    //        if (0 >= Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = 1;

    //        if ((int)ViewState["PAGENUMBER"] == 0)
    //            ViewState["PAGENUMBER"] = 1;

    //        txtnopage.Text = ViewState["PAGENUMBER"].ToString();
    //    }
    //    BindData();
    //    SetPageNavigator();
    //}

    //protected void PagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    gvOffshoreSubCategory.SelectedIndex = -1;
    //    gvOffshoreSubCategory.EditIndex = -1;
    //    if (ce.CommandName == "prev")
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //    else
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

    //    BindData();
    //    SetPageNavigator();
    //}

    //private void SetPageNavigator()
    //{
    //    cmdPrevious.Enabled = IsPreviousEnabled();
    //    cmdNext.Enabled = IsNextEnabled();
    //    lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
    //    lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
    //    lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    //}

    //private Boolean IsPreviousEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    //private Boolean IsNextEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}
    //private void ShowNoRecordsFound(DataTable dt, GridView gv)
    //{
    //    dt.Rows.Add(dt.NewRow());
    //    gv.DataSource = dt;
    //    gv.DataBind();

    //    int colcount = gv.Columns.Count;
    //    gv.Rows[0].Cells.Clear();
    //    gv.Rows[0].Cells.Add(new TableCell());
    //    gv.Rows[0].Cells[0].ColumnSpan = colcount;
    //    gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
    //    gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
    //    gv.Rows[0].Cells[0].Font.Bold = true;
    //    gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    //    gv.Rows[0].Attributes["onclick"] = "";
    //}

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvOffshoreSubCategory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOffshoreSubCategory.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOffshoreSubCategory_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidData(((RadComboBox)e.Item.FindControl("ddlCategoryAdd")).SelectedValue
                    , ((RadTextBox)e.Item.FindControl("txtSubCategoryAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewOffshoreTrainingSubCategory.InsertTrainingSubCategory(General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlCategoryAdd")).SelectedValue)
                    , ((RadTextBox)e.Item.FindControl("txtSubCategoryAdd")).Text);
                BindData();
                ((RadComboBox)e.Item.FindControl("ddlCategoryAdd")).Focus();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                int SubCategoryid = int.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDSUBCATEGORYID"].ToString());
                PhoenixCrewOffshoreTrainingSubCategory.DeleteTrainingSubCategory(SubCategoryid);
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidData(((RadComboBox)e.Item.FindControl("ddlCategoryEdit")).SelectedValue
                    , ((RadTextBox)e.Item.FindControl("txtSubCategoryEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                int SubCategoryid = int.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDSUBCATEGORYID"].ToString());

                PhoenixCrewOffshoreTrainingSubCategory.UpdateTrainingSubCategory(SubCategoryid
                    , General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlCategoryEdit")).SelectedValue)
                     , ((RadTextBox)e.Item.FindControl("txtSubCategoryEdit")).Text);

               // _gridView.EditIndex = -1;
                BindData();
                gvOffshoreSubCategory.Rebind();

            }
            //   SetPageNavigator();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOffshoreSubCategory_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            BindData();
            //  SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOffshoreSubCategory_ItemDataBound(object sender, GridItemEventArgs e)
    {
        

        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadComboBox ddlCategoryEdit = (RadComboBox)e.Item.FindControl("ddlCategoryEdit");
            RadLabel lblCategoryId = (RadLabel)e.Item.FindControl("lblCategoryID");

            if (ddlCategoryEdit != null)
            {
                DataSet ds = PhoenixCrewOffshoreTrainingCategory.ListTrainingCategory(null);
                ddlCategoryEdit.DataTextField = "FLDCATEGORYNAME";
                ddlCategoryEdit.DataValueField = "FLDCATEGORYID";
                ddlCategoryEdit.DataSource = ds;
                ddlCategoryEdit.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
                ddlCategoryEdit.DataBind();

            }
            if (lblCategoryId != null && lblCategoryId.Text != "")
                ddlCategoryEdit.SelectedValue = lblCategoryId.Text;

            ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;

                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            ImageButton eb = (ImageButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }

            ImageButton cmdMap = (ImageButton)e.Item.FindControl("cmdMap");
            if (cmdMap != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdMap.CommandName))
                    cmdMap.Visible = false;
                cmdMap.Attributes.Add("onclick", "javascript:Openpopup('chml','','../CrewOffshore/CrewOffshoreTrainingSubCategoryCourseMapping.aspx?subcategoryid=" + drv["FLDSUBCATEGORYID"].ToString() + "'); return true;");
            }
            ImageButton cmdvsltyp = (ImageButton)e.Item.FindControl("cmdvsltyp");
            if (cmdvsltyp != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdvsltyp.CommandName))
                    cmdvsltyp.Visible = false;
                cmdvsltyp.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreCompetencyVesselMapping.aspx?CID=" + drv["FLDSUBCATEGORYID"].ToString() + "'); return true;");
            }
        }
        if (e.Item is GridFooterItem)
        {
            RadComboBox ddlCategoryAdd = (RadComboBox)e.Item.FindControl("ddlCategoryAdd");
            if (ddlCategoryAdd != null)
            {
                DataSet ds = PhoenixCrewOffshoreTrainingCategory.ListTrainingCategory(null);
                ddlCategoryAdd.DataTextField = "FLDCATEGORYNAME";
                ddlCategoryAdd.DataValueField = "FLDCATEGORYID";
                ddlCategoryAdd.DataSource = ds;
                ddlCategoryAdd.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
                ddlCategoryAdd.DataBind();

            }
            ImageButton ab = (ImageButton)e.Item.FindControl("cmdAdd");
            if (ab != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName))
                    ab.Visible = false;
            }
        }
    }
}
