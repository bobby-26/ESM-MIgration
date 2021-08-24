using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class CrewCourseandCertificateArchived : PhoenixBasePage
{
    string empid = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            empid = Request.QueryString["empid"];

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Courses", "OC");
            toolbar.AddButton("CBT Courses", "CBT");
            toolbar.AddButton("EPSS", "EPSS");
            MenuCrew.AccessRights = this.ViewState;
            MenuCrew.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
               

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["ODPAGENUMBER"] = 1;
                ViewState["ODSORTEXPRESSION"] = null;
                ViewState["ODSORTDIRECTION"] = null;
                ViewState["EPSSPAGENUMBER"] = 1;
                ViewState["EPSSSORTEXPRESSION"] = null;
                ViewState["EPSSSORTDIRECTION"] = null;

                gvCrewCourseCertificate.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvCBTCourses.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvEpss.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            if (Request.QueryString["type"] == "1")
            {
                MenuCrew.SelectedMenuIndex = 0;
                gvCBTCourses.Visible = false;
                gvEpss.Visible = false;             
            }
            else if (Request.QueryString["type"] == "2")
            {
                MenuCrew.SelectedMenuIndex = 1;
                gvCrewCourseCertificate.Visible = false;
                gvEpss.Visible = false;                
            }
            else if (Request.QueryString["type"] == "3")
            {
                MenuCrew.SelectedMenuIndex = 2;
                gvCBTCourses.Visible = false;
                gvCrewCourseCertificate.Visible = false;                

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Crew_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("OC"))
            {
                Response.Redirect("CrewCourseandCertificateArchived.aspx?empid=" + empid + "&type=1", false);
            }
            else if (CommandName.ToUpper().Equals("CBT"))
            {
                Response.Redirect("CrewCourseandCertificateArchived.aspx?empid=" + empid + "&type=2", false);
            }
            else if (CommandName.ToUpper().Equals("EPSS"))
            {
                Response.Redirect("CrewCourseandCertificateArchived.aspx?empid=" + empid + "&type=3", false);
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

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewCourseCertificate.CrewCourseCertificateSearch(
                        Int32.Parse(empid), 0
                        , 0  // list courses other than cbt's
                        , sortexpression, sortdirection
                        , (int)ViewState["PAGENUMBER"]
                        , gvCrewCourseCertificate.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount);

            gvCrewCourseCertificate.DataSource = ds;
            gvCrewCourseCertificate.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewCourseCertificate_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewCourseCertificate.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvCrewCourseCertificate_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdArchive");
            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'De-Archive selected document ?')");
                if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName)) db1.Visible = false;
            }

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");

            if (att != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;
                if (lblIsAtt.Text == "0")
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }

                att.Attributes.Add("onclick", "javascript:openNewWindow('cca','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.COURSE + "&u=n'); return false;");

            }

            System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
            RadLabel expdate = e.Item.FindControl("lblExpiryDate") as RadLabel;
            DateTime? d = General.GetNullableDateTime(expdate.Text);
            if (expdate != null)
            {
                if (d.HasValue)
                {
                    TimeSpan t = d.Value - DateTime.Now;
                    if (t.Days >= 0 && t.Days < 120)
                    {
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/yellow.png";
                    }
                    else if (t.Days < 0)
                    {
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/red.png";
                    }
                }
            }

        }
    }

    protected void gvCrewCourseCertificate_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "DEARCHIVE")
            {
                string id = ((RadLabel)e.Item.FindControl("lblCourseId")).Text;
                if (Request.QueryString["t"] == "n")
                    PhoenixNewApplicantCourse.ArchiveCrewCourseCertificate(int.Parse(empid), int.Parse(id), 1);
                else
                    PhoenixCrewCourseCertificate.ArchiveCrewCourseCertificate(int.Parse(empid), int.Parse(id), 1);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);

                BindData();
                gvCrewCourseCertificate.Rebind();
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

    protected void gvCrewCourseCertificate_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string coursecertificateid = ((RadLabel)e.Item.FindControl("lblCourseId")).Text;

            PhoenixCrewCourseCertificate.DeleteCrewCourseCertificate(Convert.ToInt32(coursecertificateid)
                                                            );
            BindData();
            gvCrewCourseCertificate.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    private void BindODData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["ODSORTEXPRESSION"] == null) ? null : (ViewState["ODSORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["ODSORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["ODSORTDIRECTION"].ToString());


            DataSet ds = PhoenixCrewCourseCertificate.CrewCourseCertificateSearch(
                                Int32.Parse(empid), 0
                                , 1,  // list CBT courses only
                               sortexpression, sortdirection,
                               (int)ViewState["ODPAGENUMBER"],
                               gvCBTCourses.PageSize,
                               ref iRowCount,
                               ref iTotalPageCount);

            gvCBTCourses.DataSource = ds;
            gvCBTCourses.VirtualItemCount = iRowCount;

            ViewState["ODROWCOUNT"] = iRowCount;
            ViewState["ODTOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCBTCourses_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["ODPAGENUMBER"] = ViewState["ODPAGENUMBER"] != null ? ViewState["ODPAGENUMBER"] : gvCBTCourses.CurrentPageIndex + 1;
        BindODData();

    }

    protected void gvCBTCourses_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "DEARCHIVE")
            {
                string id = ((RadLabel)e.Item.FindControl("lblCourseId")).Text;
                if (Request.QueryString["t"] == "n")
                    PhoenixNewApplicantCourse.ArchiveCrewCourseCertificate(int.Parse(empid), int.Parse(id), 1);
                else
                    PhoenixCrewCourseCertificate.ArchiveCrewCourseCertificate(int.Parse(empid), int.Parse(id), 1);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);
                BindODData();
                gvCBTCourses.Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["ODPAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCBTCourses_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string coursecertificateid = ((RadLabel)e.Item.FindControl("lblCourseId")).Text;

            PhoenixCrewCourseCertificate.DeleteCrewCourseCertificate(Convert.ToInt32(coursecertificateid));
            BindODData();
            gvCBTCourses.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }


    protected void gvCBTCourses_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdArchive");
            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'De-Archive selected document ?')");
                if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName)) db1.Visible = false;
            }

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");

            if (att != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;

                if (lblIsAtt.Text == "0")
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }

                att.Attributes.Add("onclick", "javascript:openNewWindow('cca','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.COURSE + "&u=n'); return false;");
            }

            System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
            RadLabel expdate = e.Item.FindControl("lblExpiryDate") as RadLabel;
            DateTime? d = General.GetNullableDateTime(expdate.Text);
            if (expdate != null)
            {
                if (d.HasValue)
                {
                    TimeSpan t = d.Value - DateTime.Now;
                    if (t.Days >= 0 && t.Days < 120)
                    {
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/yellow.png";
                    }
                    else if (t.Days < 0)
                    {
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/red.png";
                    }
                }
            }

        }
    }

    protected void gvCBTCourses_SortCommand(object sender, GridSortCommandEventArgs e)
    {

    }

    private void BindEpssData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["EPSSSORTEXPRESSION"] == null) ? null : (ViewState["EPSSSORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["EPSSSORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["EPSSSORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewCourseCertificate.CrewCourseCertificateSearch(
                        Int32.Parse(empid), 0
                        , 2  // list EPSS courses 
                        , sortexpression, sortdirection
                        , (int)ViewState["EPSSPAGENUMBER"]
                        , gvEpss.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount);
            
            gvEpss.DataSource = ds;
            gvEpss.VirtualItemCount = iRowCount;
            
            ViewState["EPSSROWCOUNT"] = iRowCount;
            ViewState["EPSSTOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }




    protected void gvEpss_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["EPSSPAGENUMBER"] = ViewState["EPSSPAGENUMBER"] != null ? ViewState["EPSSPAGENUMBER"] : gvEpss.CurrentPageIndex + 1;
        BindEpssData();
    }

   
    protected void gvEpss_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {    
            if (e.CommandName.ToString().ToUpper() == "DEARCHIVE")
            {
                string id = ((RadLabel)e.Item.FindControl("lblCourseId")).Text;
                if (Request.QueryString["t"] == "n")
                    PhoenixNewApplicantCourse.ArchiveCrewCourseCertificate(int.Parse(empid), int.Parse(id), 1);
                else
                    PhoenixCrewCourseCertificate.ArchiveCrewCourseCertificate(int.Parse(empid), int.Parse(id), 1);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);
               
                BindEpssData();
                gvEpss.Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["EPSSPAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEpss_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string coursecertificateid = ((RadLabel)e.Item.FindControl("lblCourseId")).Text;

            PhoenixCrewCourseCertificate.DeleteCrewCourseCertificate(Convert.ToInt32(coursecertificateid)
                                                            );
            BindEpssData();
            gvEpss.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvEpss_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdArchive");
            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'De-Archive selected document ?')");
                if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName)) db1.Visible = false;
            }

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");

            if (att != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;
                
                if (lblIsAtt.Text == "0")
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }

                att.Attributes.Add("onclick", "javascript:openNewWindow('cca','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.COURSE + "&u=n'); return false;");
                
            }

            System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
            RadLabel expdate = e.Item.FindControl("lblExpiryDate") as RadLabel;
            DateTime? d = General.GetNullableDateTime(expdate.Text);
            if (expdate != null)
            {
                if (d.HasValue)
                {
                    TimeSpan t = d.Value - DateTime.Now;
                    if (t.Days >= 0 && t.Days < 120)
                    {
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/yellow.png";
                    }
                    else if (t.Days < 0)
                    {
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/red.png";
                    }
                }
            }

        }
    }
    

}
