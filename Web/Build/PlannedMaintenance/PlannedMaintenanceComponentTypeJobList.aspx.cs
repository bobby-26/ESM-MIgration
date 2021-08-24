using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;

public partial class PlannedMaintenanceComponentTypeJobList : PhoenixBasePage
{
       protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../PlannedMaintenance/PlannedMaintenanceComponentTypeJobList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvComponentTypeJob')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../PlannedMaintenance/PlannedMaintenanceComponentTypeJobList.aspx?" + (Request.QueryString["tv"] != null ? "tv=1" : string.Empty), "Add Jobs", "add.png", "Add");
            toolbargrid.AddImageButton("../PlannedMaintenance/PlannedMaintenanceComponentTypeJobList.aspx?" + (Request.QueryString["tv"] != null ? "tv=1" : string.Empty), "View Jobs", "view-task.png", "View");
            
            //toolbargrid.AddButton("Add", "New");
            //toolbargrid.AddButton("View", "View");
            MenuDivComponentTypeJob.MenuList = toolbargrid.Show();
            MenuDivComponentTypeJob.SetTrigger(pnlComponentType);

            if (!IsPostBack)
            {
                
                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

                ViewState["ISTREENODECLICK"] = false; 
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["COMPONENTTYPEID"] = null;
                ViewState["COMPONENTTYPEJOBID"] = null; 
                ViewState["SETCURRENTNAVIGATIONTAB"] = null;
                if (Request.QueryString["COMPONENTTYPEID"] != null)
                    ViewState["COMPONENTTYPEID"] = Request.QueryString["COMPONENTTYPEID"].ToString();


                if (Request.QueryString["COMPONENTTYPEJOBID"] != null)
                {
                    ViewState["COMPONENTTYPEJOBID"] = Request.QueryString["COMPONENTTYPEJOBID"].ToString();
                }                
               
            }
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }    
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDJOBCODE", "FLDJOBTITLE", "FLDFREQUENCYNAME", "FLDPRIORITY", };
            string[] alCaptions = { "Job Code", "Job Title", "Frequency", "Priority" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());      
            DataSet ds = PhoenixPlannedMaintenanceComponentTypeJob.ComponentTypeJobSearch( General.GetNullableGuid(ViewState["COMPONENTTYPEID"].ToString()), sortexpression, sortdirection,
                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                        General.ShowRecords(null),
                        ref iRowCount,
                        ref iTotalPageCount);
            General.ShowExcel("Components Type Jobs", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuDivComponentTypeJob_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                SetPageNavigator();
            }
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (dce.CommandName.ToUpper().Equals("ADD"))
            {
                ClientScript.RegisterStartupScript(GetType(), "Load", "<script type='text/javascript'>window.parent.location.href = '../PlannedMaintenance/PlannedMaintenanceComponentTypeJob.aspx?" + (Request.QueryString["tv"] != null ? "tv=1&" : string.Empty) + "COMPONENTTYPEID=" + ViewState["COMPONENTTYPEID"].ToString() + "'; </script>");
               //Response.Redirect("../PlannedMaintenance/PlannedMaintenanceComponentTypeJob.aspx?COMPONENTTYPEID=" + ViewState["COMPONENTTYPEID"].ToString());
            }
            if (dce.CommandName.ToUpper().Equals("VIEW"))
            {

                ClientScript.RegisterStartupScript(GetType(), "Load", "<script type='text/javascript'>window.parent.location.href = '../PlannedMaintenance/PlannedMaintenanceComponentTypeJob.aspx?" + (Request.QueryString["tv"] != null ? "tv=1&" : string.Empty) + "COMPONENTTYPEID=" + ViewState["COMPONENTTYPEID"].ToString() + "'; </script>");
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDJOBCODE", "FLDJOBTITLE", "FLDFREQUENCYNAME", "FLDPRIORITY", };
            string[] alCaptions = { "Job Code", "Job Title", "Frequency", "Priority" };        

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds;
            ds = PhoenixPlannedMaintenanceComponentTypeJob.ComponentTypeJobSearch(General.GetNullableGuid(ViewState["COMPONENTTYPEID"].ToString()) , sortexpression, sortdirection,
                         Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                         General.ShowRecords(null),
                         ref iRowCount,
                         ref iTotalPageCount);
            General.SetPrintOptions("gvComponentTypeJob", "Components Type Jobs", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvComponentTypeJob.DataSource = ds;
                gvComponentTypeJob.DataBind();

                if (ViewState["COMPONENTTYPEJOBID"] == null) 
                {
                    ViewState["COMPONENTTYPEJOBID"] = ds.Tables[0].Rows[0]["FLDCOMPONENTTYPEJOBID"].ToString();
                    ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                    gvComponentTypeJob.SelectedIndex = 0; 
                }             
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvComponentTypeJob);                
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvComponentTypeJob_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["STORETYPEID"] = null;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        BindData();
    }


    protected void cmdSort_Click(object sender, EventArgs e)
    {
        try
        {
            ImageButton ib = (ImageButton)sender;
            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    

    protected void gvComponentTypeJob_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvComponentTypeJob_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixPlannedMaintenanceComponentTypeJob.DeleteComponentTypeJob(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(((Label)_gridView.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblComponentTypeJobId")).Text));
                _gridView.EditIndex = -1;
                ViewState["COMPONENTTYPEJOBID"] = null;
                BindData();
                SetPageNavigator();
            }            

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvComponentTypeJob_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        gvComponentTypeJob.SelectedIndex = se.NewSelectedIndex;

        ViewState["COMPONENTTYPEID"] = ((Label)gvComponentTypeJob.Rows[se.NewSelectedIndex].FindControl("lblComponentTypeId")).Text;
        ViewState["JOBID"] = ((Label)gvComponentTypeJob.Rows[se.NewSelectedIndex].FindControl("lblJobID")).Text;
        ViewState["COMPONENTTYPEJOBID"] = ((Label)gvComponentTypeJob.Rows[se.NewSelectedIndex].FindControl("lblComponentTypeJobId")).Text;
        ViewState["DTKEY"] = ((Label)gvComponentTypeJob.Rows[se.NewSelectedIndex].FindControl("lbldtkey")).Text;        
    }

   
     

    protected void gvComponentTypeJob_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        try
        {
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvComponentTypeJob_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvComponentTypeJob_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (ViewState["SORTEXPRESSION"] != null)
                {
                    HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                    if (img != null)
                    {
                        if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                            img.Src = Session["images"] + "/arrowUp.png";
                        else
                            img.Src = Session["images"] + "/arrowDown.png";

                        img.Visible = true;
                    }
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                ImageButton jd = (ImageButton)e.Row.FindControl("cmdJobDesc");
                jd.Attributes.Add("onclick", "return showPickList('spnPickReason', 'codehelp1', '', '../PlannedMaintenance/PlannedMaintenanceComponentTypeJobDetail.aspx?framename=ifMoreInfo&COMPONENTTYPEJOBID=" + drv["FLDCOMPONENTTYPEJOBID"].ToString() + "', true);");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        try
        {

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        try
        {
            int result;
            if (Int32.TryParse(txtnopage.Text, out result))
            {
                ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

                if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                    ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

                if (0 >= Int32.Parse(txtnopage.Text))
                    ViewState["PAGENUMBER"] = 1;

                if ((int)ViewState["PAGENUMBER"] == 0)
                    ViewState["PAGENUMBER"] = 1;

                txtnopage.Text = ViewState["PAGENUMBER"].ToString();
            }
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvComponentTypeJob.SelectedIndex = -1;
            gvComponentTypeJob.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            BindData();           
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetPageNavigator()
    {

        try
        {
            cmdPrevious.Enabled = IsPreviousEnabled();
            cmdNext.Enabled = IsNextEnabled();
            lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
            lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
            lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private Boolean IsPreviousEnabled()
    {

        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;


    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;


    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
        {
            dt.Rows.Add(dt.NewRow());
            gv.DataSource = dt;
            gv.DataBind();

            int colcount = gv.Columns.Count;
            gv.Rows[0].Cells.Clear();
            gv.Rows[0].Cells.Add(new TableCell());
            gv.Rows[0].Cells[0].ColumnSpan = colcount;
            gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
            gv.Rows[0].Cells[0].Font.Bold = true;
            gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
            gv.Rows[0].Attributes["onclick"] = "";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }   
}
