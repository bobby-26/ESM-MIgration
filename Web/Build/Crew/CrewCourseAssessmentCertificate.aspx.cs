using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class CrewCourseAssessmentCertificate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Generate Certificate","CERTIFICATE");
            toolbar.AddButton("Generate LOA", "LOA");
            MenuCrewCourseCertificate.AccessRights = this.ViewState;
            MenuCrewCourseCertificate.MenuList = toolbar.Show();
            if (Filter.CurrentCourseSelection != null)
            {
                EditCourseDetails();
            }
            if (Request.QueryString["batchid"] != null)
            {
                EditBatchDetails(Convert.ToInt32(Request.QueryString["batchid"]));
            }
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                if (Session["COURSEID"] != null)
                {
                    EditCourseDetails();
                }
				BindData();
			
            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void EditCourseDetails()
    {
        try
        {

            int courseid = Convert.ToInt32(Filter.CurrentCourseSelection);
            DataSet ds = PhoenixRegistersDocumentCourse.EditDocumentCourse(courseid);
            if (ds.Tables[0].Rows.Count > 0)
            {

                ucCourse.SelectedCourse = ds.Tables[0].Rows[0]["FLDDOCUMENTID"].ToString();
                ucCourseType.SelectedHard = ds.Tables[0].Rows[0]["FLDDOCUMENTTYPE"].ToString();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void EditBatchDetails(int batchid)
    {
        try
        {

            DataSet ds = PhoenixRegistersBatch.EditBatch(batchid);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtBatchNo.Text = ds.Tables[0].Rows[0]["FLDBATCHNAME"].ToString();
                txtFromDate.Text = ds.Tables[0].Rows[0]["FLDFROMDATE"].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDFROMDATE"].ToString()) : ds.Tables[0].Rows[0]["FLDFROMDATE"].ToString();
                txtToDate.Text = ds.Tables[0].Rows[0]["FLDTODATE"].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDTODATE"].ToString()) : ds.Tables[0].Rows[0]["FLDTODATE"].ToString();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void EditDateDetails(int batchid)
    {

    }
    protected void CrewCertificate_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            string batchid = Request.QueryString["batchid"].ToString();
            string empid = "";
            string eid = "";
            if (dce.CommandName.ToUpper().Equals("CERTIFICATE"))
            {             
                for (int i = 0; i < gvCertificateList.Rows.Count; i++)
                {
                    CheckBox c = (CheckBox)gvCertificateList.Rows[i].FindControl("chkCertificate");
                    if (c.Checked)
                    {
                        eid = ((Label)gvCertificateList.Rows[i].FindControl("lblEmployeeId")).Text;
                        empid = empid + eid + ","; 
                    }
                }
                if (empid.Length > 0)
                {
                    empid = empid.Substring(0, empid.Length - 1);
                }
                if (empid.Length == 0)
                {
                    ucError.ErrorMessage = "Please select the employees for whom you want to generate the Certificate.";
                    ucError.Visible = true;
                    return;
                }
				Response.Redirect("../Reports/ReportsView.aspx?applicationcode=4&reportcode=COURSECERTIFICATE&batchid=" + batchid + "&empid=" + empid + "&showmenu=1", false);
            }
            if (dce.CommandName.ToUpper().Equals("LOA"))
            {                  
                for (int i = 0; i < gvCertificateList.Rows.Count; i++)
                {
                    CheckBox cl = (CheckBox)gvCertificateList.Rows[i].FindControl("chkLoa");
                    if (cl.Checked)
                    {
                        eid = ((Label)gvCertificateList.Rows[i].FindControl("lblEmployeeId")).Text;
                        empid = empid + eid + ",";
                    }
                }
                if (empid.Length > 0)
                {
                    empid = empid.Substring(0, empid.Length - 1);
                }
                if (empid.Length == 0)
                {
                    ucError.ErrorMessage = "Please select the employees for whom you want to generate the LOA.";
                    ucError.Visible = true;
                    return;
                }
                Response.Redirect("../Reports/ReportsView.aspx?applicationcode=4&reportcode=COURSELOA&batchid=" + batchid + "&empid=" + empid + "&showmenu=1", false);
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

        DataSet ds = PhoenixCrewCourseAssessment.CrewCertificateGeneration(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(Filter.CurrentCourseSelection), General.GetNullableInteger(Request.QueryString["batchid"]));

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCertificateList.DataSource = ds;
            gvCertificateList.DataBind();

        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvCertificateList);
        }
    }
	protected void CheckAll(Object sender,EventArgs e)
	{
		CheckBox chkAll = (CheckBox)gvCertificateList.HeaderRow.FindControl("chkAllCertificate");
		foreach (GridViewRow row in gvCertificateList.Rows)
		{
			CheckBox cbSelected = (CheckBox)row.FindControl("chkCertificate");
			string lbl1 = ((Label)row.FindControl("lblCertificate")).Text;
			string lblResult = ((Label)row.FindControl("lblResult")).Text;
			if (chkAll.Checked == true)
			{
				if (lbl1 == "1")
				{
					cbSelected.Checked = true;
				}
			}
			else
			{
				cbSelected.Checked = false;
			}
			if (lblResult == "F")
			{
				cbSelected.Checked = false;
			}
		}
	}
	protected void CheckAllLOA(Object sender, EventArgs e)
	{
		CheckBox chkAll = (CheckBox)gvCertificateList.HeaderRow.FindControl("chkAllLOA");
		foreach (GridViewRow row in gvCertificateList.Rows)
		{
			CheckBox cbSelected = (CheckBox)row.FindControl("chkLoa");
			string lbl1 = ((Label)row.FindControl("lblCertificate")).Text;
			string lblResult = ((Label)row.FindControl("lblResult")).Text;
			if (chkAll.Checked == true)
			{
				if (lbl1 != "1")
				{
					cbSelected.Checked = true;
				}
			}
			else
			{
				cbSelected.Checked = false;
			}
			if (lblResult == "F")
			{
				cbSelected.Checked = false;
			}
		}
	}
    public void gvCertificateList_ItemDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                string lbl1 = ((Label)e.Row.FindControl("lblCertificate")).Text;
				string lblResult = ((Label)e.Row.FindControl("lblResult")).Text;
                CheckBox chk1 = (CheckBox)e.Row.FindControl("chkCertificate");
               // CheckBox chk2 = (CheckBox)e.Row.FindControl("chkLoa");

				if (lbl1 == "1" )
				{
				//	chk2.Enabled = false;
				}
				else
				{
					e.Row.CssClass = "redfont";
					chk1.Enabled = false;
					//chk2.Checked = false;
				}
				if (lblResult == "F")
				{
					//chk2.Enabled = false;
					chk1.Enabled = false;
				}
            }
        }
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
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

}
