using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using SouthNests.Phoenix.PlannedMaintenance;

public partial class PlannedMaintenanceComponentTypeJob : PhoenixBasePage
{
       protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
           

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../PlannedMaintenance/PlannedMaintenanceComponentTypeJob.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvComponentTypeJob')", "Print Grid", "icon_print.png", "PRINT");
            MenuDivComponentTypeJob.MenuList = toolbargrid.Show();
            MenuDivComponentTypeJob.SetTrigger(pnlComponentType);

            if (!IsPostBack)
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("General", "GENERAL");
                toolbarmain.AddButton("Job Description", "DETAILS");
                toolbarmain.AddButton("Attachment", "ATTACHMENT");
                toolbarmain.AddButton("ComponentType", "COMPONENT");
                MenuComponentTypeJob.MenuList = toolbarmain.Show();
                MenuComponentTypeJob.SetTrigger(pnlComponentType);
                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

                ViewState["ISTREENODECLICK"] = false; 
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["COMPONENTTYPEID"] = "";
                ViewState["COMPONENTTYPEJOBID"] = ""; 
                ViewState["SETCURRENTNAVIGATIONTAB"] = null;
                if (Request.QueryString["COMPONENTTYPEID"] != null)
                {
                    ViewState["COMPONENTTYPEID"] = Request.QueryString["COMPONENTTYPEID"].ToString();                   
                }

                if (Request.QueryString["COMPONENTTYPEJOBID"] != null)
                {
                    ViewState["COMPONENTTYPEJOBID"] = Request.QueryString["COMPONENTTYPEJOBID"].ToString();
                }
               
                MenuComponentTypeJob.SelectedMenuIndex = 0;
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

  

 

    protected void MenuComponentTypeJob_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            PhoenixInventorySpareItem objinventorystockitem = new PhoenixInventorySpareItem();

            if (dce.CommandName.ToUpper().Equals("GENERAL"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceComponentTypeJobGeneral.aspx?COMPONENTTYPEID="+ViewState["COMPONENTTYPEID"].ToString()+"&COMPONENTTYPEJOBID= ";
            }
            else if (dce.CommandName.ToUpper().Equals("DETAILS"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceComponentTypeJobDetail.aspx?COMPONENTTYPEJOBID=";
            }
            else if (dce.CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.INVENTORY;
            }
            else if (dce.CommandName.ToUpper().Equals("COMPONENT"))
            {
                Response.Redirect("../Inventory/InventoryComponentType.aspx?" + (Request.QueryString["tv"] != null ? "tv=1&" : string.Empty) + "COMPONENTTYPEID=" + ViewState["COMPONENTTYPEID"].ToString() + "&SETCURRENTNAVIGATIONTAB=../PlannedMaintenance/PlannedMaintenanceComponentTypeJobList.aspx", false);
            }
            if (dce.CommandName.ToUpper().Equals("ATTACHMENT"))
            { 
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString(); 
            }
            else
            {
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["COMPONENTTYPEJOBID"];
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDJOBCODE", "FLDJOBTITLE", "FLDFREQUENCYNAME", "FLDPRIORITY",  };
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
            DataSet ds = PhoenixPlannedMaintenanceComponentTypeJob.ComponentTypeJobSearch(General.GetNullableGuid(ViewState["COMPONENTTYPEID"].ToString()), sortexpression, sortdirection,
                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                        General.ShowRecords(null),
                        ref iRowCount,
                        ref iTotalPageCount);

            Response.AddHeader("Content-Disposition", "attachment; filename=ComponentTypeJob.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>ComponentType Job</h3></td>");
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
            General.SetPrintOptions("gvComponentTypeJob", "ComponentType Job", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvComponentTypeJob.DataSource = ds;
                gvComponentTypeJob.DataBind();

                if (ViewState["COMPONENTTYPEJOBID"] == null || ViewState["COMPONENTTYPEJOBID"].ToString() == "") 
                {
                    ViewState["COMPONENTTYPEJOBID"] = ds.Tables[0].Rows[0]["FLDCOMPONENTTYPEJOBID"].ToString();
                    ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                    gvComponentTypeJob.SelectedIndex = 0; 
                }

                if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceComponentTypeJobGeneral.aspx?COMPONENTTYPEID=" + ViewState["COMPONENTTYPEID"].ToString() + "&COMPONENTTYPEJOBID=";
                }

                if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("CommonFileAttachment.aspx"))
                {
                    ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.PLANNEDMAINTENANCE;
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceComponentTypeJobGeneral.aspx?COMPONENTTYPEID=" + ViewState["COMPONENTTYPEID"].ToString() + "&COMPONENTTYPEJOBID=" + ViewState["COMPONENTTYPEJOBID"] + "";
                }
                SetRowSelection();
                SetTabHighlight();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvComponentTypeJob);
                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceComponentTypeJobGeneral.aspx?COMPONENTTYPEID=" + ViewState["COMPONENTTYPEID"].ToString();
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

        ViewState["COMPONENTTYPEID"] = null;


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
         ComponentTypeTypeNameClick();

    }

    protected void ComponentTypeTypeNameClick()
    {
        try
        {
            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("CommonFileAttachment.aspx"))
            {
                ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.INVENTORY;
            }
            else
            {
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["COMPONENTTYPEJOBID"];
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

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
            ComponentTypeTypeNameClick();
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


    protected void SetTabHighlight()
    {
        try
        {
            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceComponentTypeJobGeneral.aspx"))
            {
                MenuComponentTypeJob.SelectedMenuIndex = 0;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceComponentTypeJobDetail.aspx"))
            {
                MenuComponentTypeJob.SelectedMenuIndex = 1;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("CommonFileAttachment.aspx"))
            {
                MenuComponentTypeJob.SelectedMenuIndex = 2;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    private void SetRowSelection()
    {
        gvComponentTypeJob.SelectedIndex = -1;
        for (int i = 0; i < gvComponentTypeJob.Rows.Count; i++)
        {
            if (gvComponentTypeJob.DataKeys[i].Value.ToString().Equals(ViewState["COMPONENTTYPEJOBID"].ToString()))
            {
                gvComponentTypeJob.SelectedIndex = i;               
                ViewState["DTKEY"] = ((Label)gvComponentTypeJob.Rows[gvComponentTypeJob.SelectedIndex].FindControl("lbldtkey")).Text;
                ViewState["JOBID"] = ((Label)gvComponentTypeJob.Rows[gvComponentTypeJob.SelectedIndex].FindControl("lblJobID")).Text;
            }
        }
    }

}
