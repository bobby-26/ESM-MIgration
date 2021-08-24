using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Text;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class CrewReportCoursesDoneFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                ViewState["DEPT"] = "";
                ViewState["LEVEL"] = "";
                ViewState["DOCTYPE"] = "";
                ViewState["RANK"] = "";
                ViewState["VESSELTYPE"] = "";
                BindCheckBoxList();
                toolbar.AddButton("Go", "GO");
                toolbar.AddButton("Cancel", "CANCEL");
                MenuCoursesDoneReport.AccessRights = this.ViewState;                
                MenuCoursesDoneReport.MenuList = toolbar.Show();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindCheckBoxList()
    {
        cblDepartment.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 51);
        cblDepartment.DataTextField = "FLDHARDNAME";
        cblDepartment.DataValueField = "FLDHARDCODE";
        cblDepartment.DataBind();

        cblLevel.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 50);
        cblLevel.DataTextField = "FLDHARDNAME";
        cblLevel.DataValueField = "FLDHARDCODE";
        cblLevel.DataBind();

        cblCourse.DataSource = PhoenixRegistersDocumentCourse.ListDocumentCourse(General.GetNullableInteger(ViewState["DOCTYPE"].ToString()));
        cblCourse.DataTextField = "FLDCOURSE";
        cblCourse.DataValueField = "FLDDOCUMENTID";
        cblCourse.DataBind();

        cblVesselType.DataSource = PhoenixRegistersVesselType.ListVesselType(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        cblVesselType.DataTextField = "FLDTYPEDESCRIPTION";
        cblVesselType.DataValueField = "FLDVESSELTYPEID";
        cblVesselType.DataBind();

        cblRank.DataSource = PhoenixRegistersRank.ListRank();
        cblRank.DataTextField = "FLDRANKNAME";
        cblRank.DataValueField = "FLDRANKID";
        cblRank.DataBind();

    }
    protected void DocumentTypeSelection(object sender, EventArgs e)
    {
        ViewState["DOCTYPE"] = ucDocumentType.SelectedHard;
        foreach (ListItem item in cblCourse.Items)
        {
            item.Selected = false;

        }
        if (ViewState["DOCTYPE"].ToString() != "" && ucDocumentType.SelectedHard.ToUpper() != "DUMMY")
        {
            DataSet ds = PhoenixRegistersDocumentCourse.ListDocumentCourseFilter(int.Parse(ViewState["DOCTYPE"].ToString()), General.GetNullableString(ViewState["RANK"].ToString()), General.GetNullableString(ViewState["VESSELTYPE"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                string[] courses = dr["FLDCOURSEFILTER"].ToString().Split(',');
                foreach (string item in courses)
                {
                    if (item.Trim() != "")
                    {
                        cblCourse.Items.FindByValue(item).Selected = true;
                    }

                }
            }
        }
    }

    protected void DepartmentSelection(object sender, EventArgs e)
    {
        StringBuilder strdepartment = new StringBuilder();
        foreach (ListItem item in cblDepartment.Items)
        {
            if (item.Selected == true)
            {
                strdepartment.Append(item.Value.ToString());
                strdepartment.Append(",");
            }
        }
        if (strdepartment.Length > 1)
        {
            strdepartment.Remove(strdepartment.Length - 1, 1);
        }

        ViewState["DEPT"] = strdepartment.ToString();
        foreach (ListItem item in cblRank.Items)
        {
            item.Selected = false;

        }
        if (ViewState["DEPT"].ToString() != "" || strdepartment.ToString() != "")
        {
            DataSet ds = PhoenixRegistersRank.ListRankFilter(General.GetNullableString(ViewState["DEPT"].ToString()), General.GetNullableString(ViewState["LEVEL"].ToString()), null, 0);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                string[] rank = dr["FLDFILTERRANK"].ToString().Split(',');
                foreach (string item in rank)
                {
                    if (item.Trim() != "")
                    {
                        cblRank.Items.FindByValue(item).Selected = true;
                    }

                }
            }
        }

    }
    protected void RankFilterSelection(object sender, EventArgs e)
    {
        StringBuilder strrank = new StringBuilder();
        foreach (ListItem item in cblRank.Items)
        {
            if (item.Selected == true)
            {
                strrank.Append(item.Value.ToString());
                strrank.Append(",");
            }
        }
        if (strrank.Length > 1)
        {
            strrank.Remove(strrank.Length - 1, 1);
        }
        ViewState["RANK"] = strrank.ToString();
        foreach (ListItem item in cblCourse.Items)
        {
            item.Selected = false;

        }
        if (ViewState["DOCTYPE"].ToString() != "" || strrank.ToString() != "" || ViewState["VESSELTYPE"].ToString() != "")
        {
            DataSet ds = PhoenixRegistersDocumentCourse.ListDocumentCourseFilter(General.GetNullableInteger(ViewState["DOCTYPE"].ToString()), strrank.ToString(),
                General.GetNullableString(ViewState["VESSELTYPE"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                string[] courses = dr["FLDCOURSEFILTER"].ToString().Split(',');
                foreach (string item in courses)
                {
                    if (item.Trim() != "")
                    {
                        cblCourse.Items.FindByValue(item).Selected = true;
                    }

                }
            }
        }
    }
    protected void VesselTypeFilterSelection(object sender, EventArgs e)
    {
        StringBuilder strvesseltype = new StringBuilder();
        foreach (ListItem item in cblVesselType.Items)
        {
            if (item.Selected == true)
            {
                strvesseltype.Append(item.Value.ToString());
                strvesseltype.Append(",");
            }
        }
        if (strvesseltype.Length > 1)
        {
            strvesseltype.Remove(strvesseltype.Length - 1, 1);
        }
        ViewState["VESSELTYPE"] = strvesseltype.ToString();
        foreach (ListItem item in cblCourse.Items)
        {
            item.Selected = false;

        }
        if (ViewState["DOCTYPE"].ToString() != "" || ViewState["RANK"].ToString() != "" || strvesseltype.ToString() != "")
        {
            DataSet ds = PhoenixRegistersDocumentCourse.ListDocumentCourseFilter(General.GetNullableInteger(ViewState["DOCTYPE"].ToString()),
                                                                                 General.GetNullableString(ViewState["RANK"].ToString()),
                                                                                 strvesseltype.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                string[] courses = dr["FLDCOURSEFILTER"].ToString().Split(',');
                foreach (string item in courses)
                {
                    if (item.Trim() != "")
                    {
                        cblCourse.Items.FindByValue(item).Selected = true;
                    }

                }
            }
        }
    }
    protected void RankSelection(object sender, EventArgs e)
    {
        StringBuilder strlevel = new StringBuilder();
        foreach (ListItem item in cblLevel.Items)
        {
            if (item.Selected == true)
            {
                strlevel.Append(item.Value.ToString());
                strlevel.Append(",");
            }
        }

        ViewState["LEVEL"] = strlevel.ToString();
        foreach (ListItem item in cblRank.Items)
        {
            item.Selected = false;

        }
        if (ViewState["DEPT"].ToString() != "" || strlevel.ToString() != "")
        {
            DataSet ds = PhoenixRegistersRank.ListRankFilter(General.GetNullableString(ViewState["DEPT"].ToString()), strlevel.ToString(), null, 0);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                string[] rank = dr["FLDFILTERRANK"].ToString().Split(',');
                foreach (string item in rank)
                {
                    if (item.Trim() != "")
                    {
                        cblRank.Items.FindByValue(item).Selected = true;
                    }

                }
            }
        }

    }
    private bool IsValidCourseFilter(string course, string fromdate, string todate)
    {
        DateTime resultdate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (course.Trim().Length == 0)
            ucError.ErrorMessage = "Select Atleast one Course.";

        if (string.IsNullOrEmpty(fromdate))
        {
            ucError.ErrorMessage = "From Date is required";
        }
        else if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From Date should be earlier than current date";
        }

        if (string.IsNullOrEmpty(todate))
        {
            ucError.ErrorMessage = "To Date is required";
        }
        else if (!string.IsNullOrEmpty(fromdate)
            && DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "To Date should be later than 'From Date'";
        }

        return (!ucError.IsError);
    }

    protected void SelectAllRank(object sender, EventArgs e)
    {
        if (chkChkAllRank.Checked == true)
        {
            foreach (ListItem item in cblRank.Items)
            {
                item.Selected = true;
            }
        }
        else
        {
            foreach (ListItem item in cblRank.Items)
            {
                item.Selected = false;
            }
        }
    }
    protected void SelectAllVesselType(object sender, EventArgs e)
    {
        if (chkChkAllVesselType.Checked == true)
        {
            foreach (ListItem item in cblVesselType.Items)
            {
                item.Selected = true;
            }
        }
        else
        {
            foreach (ListItem item in cblVesselType.Items)
            {
                item.Selected = false;
            }
        }
    }
    protected void SelectAllCourse(object sender, EventArgs e)
    {
        if (chkChkAllCourse.Checked == true)
        {
            foreach (ListItem item in cblCourse.Items)
            {
                item.Selected = true;
            }
        }
        else
        {
            foreach (ListItem item in cblCourse.Items)
            {
                item.Selected = false;
            }
        }
    }
    protected void MenuCoursesDoneReport_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        if (dce.CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();
            StringBuilder strcourse = new StringBuilder();
            foreach (ListItem item in cblCourse.Items)
            {
                if (item.Selected == true)
                {
                    strcourse.Append(item.Value.ToString());
                    strcourse.Append(",");
                }
            }
            if (strcourse.Length > 1)
            {
                strcourse.Remove(strcourse.Length - 1, 1);
            }

            if (!IsValidCourseFilter(strcourse.ToString(), txtFromDate.Text, txtToDate.Text))
            {
                ucError.Visible = true;
                return;
            }

            criteria.Clear();
            criteria.Add("courselist", strcourse.ToString());
            criteria.Add("fromdate", txtFromDate.Text);
            criteria.Add("todate", txtToDate.Text);
            Filter.CurrentCoursesDoneReportFilter = criteria;
        }
        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }

    /*  if (dce.CommandName.ToUpper().Equals("COURSESDONE"))
      {
          StringBuilder strcourse = new StringBuilder();
          foreach (ListItem item in cblCourse.Items)
          {
              if (item.Selected == true)
              {
                  strcourse.Append(item.Value.ToString());
                  strcourse.Append(",");
              }
          }
          if (strcourse.Length > 1)
          {
              strcourse.Remove(strcourse.Length - 1, 1);
          }

          if (IsValidCourseFilter(strcourse.ToString(), txtFromDate.Text, txtToDate.Text))
          {
              //Response.Redirect("../Reports/ReportsView.aspx?applicationcode=4&reportcode=COURSESDONE&courselist=" + strcourse + "&fromdate=" + txtFromDate.Text + "&todate=" + txtToDate.Text, false);
              Response.Redirect("../Crew/CrewReportCoursesDone.aspx?courselist=" + strcourse + "&fromdate=" + txtFromDate.Text + "&todate=" + txtToDate.Text, false);
          }
          else
          {
              ucError.Visible = true;
              return;
          }
      }

  }       
}*/
}
