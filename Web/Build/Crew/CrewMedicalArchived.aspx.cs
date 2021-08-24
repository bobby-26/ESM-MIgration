using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewMedicalArchived : PhoenixBasePage
{
    string strEmployeeId = string.Empty;
    string familyid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            strEmployeeId = Request.QueryString["empid"];
            familyid = Request.QueryString["familyid"];
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Medical", "MEDICAL");
            toolbar.AddButton("Medical Test", "MEDICALTEST");
            toolbar.AddButton("Vaccination", "VACCINATION");
            MenuCrewMedical.AccessRights = this.ViewState;
            MenuCrewMedical.MenuList = toolbar.Show();

            if (Request.QueryString["type"] == "1")
            {
                gvMedicalTest.Visible = false;
                gvVaccination.Visible = false;
                MenuCrewMedical.SelectedMenuIndex = 0;               
            }
            else if (Request.QueryString["type"] == "2")
            {
                gvCrewMedical.Visible = false;
                gvVaccination.Visible = false;
                MenuCrewMedical.SelectedMenuIndex = 1;               
            }
            else if (Request.QueryString["type"] == "3")
            {
                gvCrewMedical.Visible = false;
                gvMedicalTest.Visible = false;
                MenuCrewMedical.SelectedMenuIndex = 2;               
            }

            if (!IsPostBack)
            {
               
            }
            //BindMedicalData();
            //BindMedicalTestData();
            //BindMedicalVaccination();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void CrewMedical_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("MEDICAL"))
            {
                Response.Redirect("CrewMedicalArchived.aspx?empid=" + strEmployeeId + "&type=1" + (General.GetNullableInteger(familyid).HasValue ? "&familyid=" + familyid : string.Empty), false);
            }
            else if (CommandName.ToUpper().Equals("MEDICALTEST"))
            {
                Response.Redirect("CrewMedicalArchived.aspx?empid=" + strEmployeeId + "&type=2" + (General.GetNullableInteger(familyid).HasValue ? "&familyid=" + familyid : string.Empty), false);
            }
            else if (CommandName.ToUpper().Equals("VACCINATION"))
            {
                Response.Redirect("CrewMedicalArchived.aspx?empid=" + strEmployeeId + "&type=3" + (General.GetNullableInteger(familyid).HasValue ? "&familyid=" + familyid : string.Empty), false);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewMedical_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindMedicalData();
    }

    private void BindMedicalData()
    {
        try
        {
            DataSet ds = PhoenixCrewMedicalDocuments.ListCrewFlagMedical(
                        Int32.Parse(strEmployeeId), 0, General.GetNullableInteger(familyid));

            gvCrewMedical.DataSource = ds;
        
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewMedical_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "DEARCHIVE")
            {
                string Medicalid = ((RadLabel)e.Item.FindControl("lblMedicalId")).Text;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);
                PhoenixCrewMedicalDocuments.ArchiveCrewMedical(int.Parse(Medicalid), 1);
                BindMedicalData();
                gvCrewMedical.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void gvCrewMedical_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string medicalid = ((RadLabel)e.Item.FindControl("lblMedicalId")).Text;

            PhoenixCrewMedicalDocuments.DeleteCrewFlagMedical(
                Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.UserCode)
                , Convert.ToInt32(medicalid)
            );
            BindMedicalData();
            gvCrewMedical.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewMedical_SortCommand(object sender, GridSortCommandEventArgs e)
    {

    }

    protected void gvCrewMedical_ItemDataBound(object sender, GridItemEventArgs e)
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

                if (lblIsAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }

                att.Attributes.Add("onclick", "javascript:openNewWindow('Codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.MEDICAL + "&u=n'); return false;");
            }

            System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
            RadLabel expdate = e.Item.FindControl("lblDateofExpiry") as RadLabel;
            DateTime? d = General.GetNullableDateTime(expdate.Text);
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


    protected void gvMedicalTest_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindMedicalTestData();
    }



    private void BindMedicalTestData()
    {
        try
        {
            string[] alColumns = { "FLDNAMEOFMEDICAL", "FLDPLACEOFISSUE", "FLDISSUEDDATE", "FLDEXPIRYDATE" };
            string[] alCaptions = { "Medical Test", "Place Of Issue", "Issue Date", "Expiry Date" };

            DataSet ds = PhoenixCrewMedicalDocuments.ListCrewMedicalTest(
                        Int32.Parse(strEmployeeId), 0, General.GetNullableInteger(familyid));

            General.SetPrintOptions("gvMedicalTest", "Crew Medical Test", alCaptions, alColumns, ds);

            gvMedicalTest.DataSource = ds;
          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMedicalTest_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string medicaltestid = ((RadLabel)e.Item.FindControl("lblMedicalTestId")).Text;

            PhoenixCrewMedicalDocuments.DeleteCrewMedicalTest(
                Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.UserCode)
                , Convert.ToInt32(medicaltestid)
                                                            );
            BindMedicalTestData();
            gvMedicalTest.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvMedicalTest_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "DEARCHIVE")
            {
                string Medicalid = ((RadLabel)e.Item.FindControl("lblMedicalTestId")).Text;
                PhoenixCrewMedicalDocuments.ArchiveCrewMedicalTest(int.Parse(Medicalid), 1);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);

                BindMedicalTestData();
                gvMedicalTest.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }


    protected void gvMedicalTest_ItemDataBound(object sender, GridItemEventArgs e)
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

                if (lblIsAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }

                att.Attributes.Add("onclick", "javascript:openNewWindow('Codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.MEDICAL + "&u=n'); return false;");
            }


            System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
            RadLabel expdate = e.Item.FindControl("lblDateofExpiry") as RadLabel;
            DateTime? d = General.GetNullableDateTime(expdate.Text);
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

    protected void gvMedicalTest_SortCommand(object sender, GridSortCommandEventArgs e)
    {

    }


    protected void gvVaccination_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindMedicalVaccination();
    }



    private void BindMedicalVaccination()
    {
        try
        {
            string[] alColumns = { "FLDNAMEOFMEDICAL", "FLDPLACEOFISSUE", "FLDISSUEDDATE", "FLDEXPIRYDATE" };
            string[] alCaptions = { "Vaccination", "Place Of Issue", "Issue Date", "Expiry Date" };

            DataSet ds = PhoenixCrewMedicalDocuments.ListCrewVaccination(
                        Int32.Parse(strEmployeeId), 0, General.GetNullableInteger(familyid));

            General.SetPrintOptions("gvVaccination", "Crew Vaccination", alCaptions, alColumns, ds);

            gvVaccination.DataSource = ds;
       
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvVaccination_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "DEARCHIVE")
            {
                string Medicalid = ((RadLabel)e.Item.FindControl("lblCrewVaccinationId")).Text;
                PhoenixCrewMedicalDocuments.ArchiveCrewVaccination(int.Parse(Medicalid), 1);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);
                BindMedicalVaccination();
                gvVaccination.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvVaccination_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string crewvaccinationid = ((RadLabel)e.Item.FindControl("lblCrewVaccinationId")).Text;

            PhoenixCrewMedicalDocuments.DeleteCrewVaccination(
                Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.UserCode)
                , Convert.ToInt32(crewvaccinationid)
            );
            BindMedicalVaccination();
            gvVaccination.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvVaccination_ItemDataBound(object sender, GridItemEventArgs e)
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

                if (lblIsAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }

                att.Attributes.Add("onclick", "javascript:openNewWindow('Codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.MEDICAL + "&u=n'); return false;");
            }

            System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
            RadLabel expdate = e.Item.FindControl("lblDateofExpiry") as RadLabel;
            DateTime? d = General.GetNullableDateTime(expdate.Text);
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

    protected void gvVaccination_SortCommand(object sender, GridSortCommandEventArgs e)
    {

    }



}
