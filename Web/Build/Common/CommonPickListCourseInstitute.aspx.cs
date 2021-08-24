using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Common_CommonPickListCourseInstitute : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            SetInstitute();
            gvCourse.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }   
    }
    private void SetInstitute()
    {
        string strInstituteId = null;
        if (Request.QueryString["instituteId"] != null)
            strInstituteId = Request.QueryString["instituteId"].ToString();
        DataTable dt = PhoenixCrewInstitute.CrewInstituteEdit(General.GetNullableInteger(strInstituteId).Value);
        if (dt.Rows.Count > 0)
        {
            txtInstitute.Text = dt.Rows[0]["FLDINSTITUTENAME"].ToString();
            txtInstituteId.Text = strInstituteId;
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        
        DataSet ds = PhoenixCrewCourseInitiation.CrewCourseInstituteSearch(null
                                                                       , null
                                                                       , null
                                                                       , General.GetNullableInteger(txtInstituteId.Text)
                                                                       , null
                                                                       , null
                                                                       , sortexpression
                                                                       , sortdirection
                                                                       , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                       , gvCourse.PageSize
                                                                       , ref iRowCount
                                                                       , ref iTotalPageCount);


        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCourse.DataSource = ds;
            gvCourse.VirtualItemCount = iRowCount;
        }
        else
        {
            gvCourse.DataSource = "";
        }
    }

 
    protected void gvCourse_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

    }

    protected void gvCourse_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCourse.CurrentPageIndex + 1;
            BindData();           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCourse_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        string Script = "";
        NameValueCollection nvc;

        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        else if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            if (Request.QueryString["mode"] == "custom")
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
                    Script += "fnReloadList('codehelp1','ifMoreInfo',true);";
                else
                    Script += "fnReloadList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = new NameValueCollection();

                LinkButton lb = (LinkButton)e.Item.FindControl("lnkCourse");
                nvc.Add(lb.ID, lb.Text.ToString());
                RadLabel lbl = (RadLabel)e.Item.FindControl("lblCourseId");
                nvc.Add(lbl.ID, lbl.Text);
            }
            else
            {

                nvc = Filter.CurrentPickListSelection;

                LinkButton lb = (LinkButton)e.Item.FindControl("lnkCourse");
                nvc.Set(nvc.GetKey(1), lb.Text.ToString());
                RadLabel lbl = (RadLabel)e.Item.FindControl("lblCourseId");
                nvc.Set(nvc.GetKey(2), lbl.Text);


                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
                    Script += "fnClosePickList('codehelp1','',true);";
                else
                    Script += "fnClosePickList('codehelp1','');";
                Script += "</script>" + "\n";
            }

            Filter.CurrentPickListSelection = nvc;
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            // ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1',null,true);", true);
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

    }

    protected void gvCourse_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        //ViewState["SORTEXPRESSION"] = e.SortExpression;

        //if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
        //    ViewState["SORTDIRECTION"] = 1;
        //else
        //    ViewState["SORTDIRECTION"] = 0;

        //BindData();
    }
}
