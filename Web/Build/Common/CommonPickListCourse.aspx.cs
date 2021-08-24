using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using System.Text;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;

public partial class CommonPickListCourse : PhoenixBasePage
{
    string vessel;
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbarmain.AddButton("Done", "DONE", ToolBarDirection.Right);
        MenuCourse.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["vessel"]))
                vessel = Request.QueryString["vessel"].ToString();
            else
                vessel = string.Empty;
            if (Filter.CurrentPickListSelection != null)
            {
                NameValueCollection nvc = Filter.CurrentPickListSelection;
                ViewState["COURSES"] = nvc.Get(2);
            }
            gvCompletedCourse.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        //BindCompletedCourses();
        //BindData();
    }

    private void BindCompletedCourses()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDCOURSENAME", "FLDCOURSENUMBER", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDNAME", "FLDNATIONALITY", "FLDAUTHORITY", "FLDREMARKS" };
            string[] alCaptions = { "Course", "Certificate Number", "Place Of Issue", "Issue Date", "Expiry Date", "Institution", "Nationality", "Issuing Authority", "Remarks" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixCommonCrew.CrewCompletedCourseSearch(
                            General.GetNullableInteger(Request.QueryString["empid"].ToString()), null
                            , sortexpression, sortdirection
                            , 1
                            , gvCompletedCourse.PageSize
                            , ref iRowCount
                            , ref iTotalPageCount);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCompletedCourse.DataSource = ds;
                gvCompletedCourse.VirtualItemCount = iRowCount;
            }
            else
            {
                gvCompletedCourse.DataSource = "";
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
            DataTable dt = PhoenixCrewManagement.CrewAppraisalRecommendedCourseList(General.GetNullableInteger(Request.QueryString["vessel"]),
                                            General.GetNullableInteger(Request.QueryString["rankid"]));

            if (dt.Rows.Count > 0)
            {
                gvCourse.DataSource = dt;
                //gvCourse.VirtualItemCount = iRowCount;
            }
            else
            {
                gvCourse.DataSource = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuCourse_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            string Script = "";
            NameValueCollection nvc;

            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnClosePickList('codehelp1', 'ifMoreInfo');";
            Script += "</script>" + "\n";

            if (CommandName.ToUpper().Equals("DONE"))
            {
                if (gvCourse.Items.Count > 0)
                {
                    StringBuilder strCourseId = new StringBuilder();
                    StringBuilder strCourseName = new StringBuilder();

                    foreach (GridDataItem gr in gvCourse.Items)
                    {
                        RadCheckBox chkSelect = (RadCheckBox)gr.FindControl("chkSelect");
                        RadLabel lblCourseId = (RadLabel)gr.FindControl("lblCourseId");
                        RadLabel lblCourseName = (RadLabel)gr.FindControl("lblCourseName");

                        if (chkSelect.Checked == true)
                        {
                            strCourseId.Append(lblCourseId.Text);
                            strCourseId.Append(",");

                            strCourseName.Append(lblCourseName.Text);
                            strCourseName.Append(", ");
                        }
                    }
                    if (strCourseId.Length > 1)
                    {
                        strCourseId.Remove(strCourseId.Length - 1, 1);
                    }
                    if (strCourseName.Length > 1)
                    {
                        strCourseName.Remove(strCourseName.Length - 2, 2);
                    }

                    nvc = Filter.CurrentPickListSelection;

                    nvc.Set(nvc.GetKey(1), strCourseName.ToString());

                    nvc.Set(nvc.GetKey(2), strCourseId.ToString());

                    Filter.CurrentPickListSelection = nvc;
                }
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
                //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
            else if (CommandName.ToUpper().Equals("CANCEL"))
            {
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
                //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCompletedCourse_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCompletedCourse.CurrentPageIndex + 1;
            BindCompletedCourses();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCourse_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
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

    protected void gvCourse_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadCheckBox chk = (RadCheckBox)e.Item.FindControl("chkSelect");
            RadLabel lblCourseId = (RadLabel)e.Item.FindControl("lblCourseId");
            if (ViewState["COURSES"] != null && lblCourseId != null && chk != null)
            {
                string Courses = "," + ViewState["COURSES"].ToString() + ",";
                string courseid = "," + lblCourseId.Text.Trim() + ",";
                chk.Checked = Courses.Contains(courseid);
            }
        }
    }

    protected void gvCourse_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        else if (e.CommandName == "Page")
            ViewState["PAGENUMBER"] = null;
    }
}
