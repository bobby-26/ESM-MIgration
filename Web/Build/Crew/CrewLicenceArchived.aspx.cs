using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewLicenceArchived : PhoenixBasePage
{
    string strEmployeeId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            strEmployeeId = Request.QueryString["empid"];
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("National Documents", "NDS");
            toolbar.AddButton("Flag Documents", "FES");
            toolbar.AddButton("DCE", "DCE");

            MenuCrewLicence.AccessRights = this.ViewState;
            MenuCrewLicence.MenuList = toolbar.Show();

            if (Request.QueryString["type"] == "1")
            {
                MenuCrewLicence.SelectedMenuIndex = 0;
                divDCE.Visible = false;
                divFE.Visible = false;
            }
            else if (Request.QueryString["type"] == "2")
            {
                MenuCrewLicence.SelectedMenuIndex = 1;
                divLicence.Visible = false;
                divDCE.Visible = false;
            }
            else if (Request.QueryString["type"] == "3")
            {
                MenuCrewLicence.SelectedMenuIndex = 2;
                divLicence.Visible = false;
                divFE.Visible = false;
            }

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["FEPAGENUMBER"] = 1;
                ViewState["FESORTEXPRESSION"] = null;
                ViewState["FESORTDIRECTION"] = null;
                ViewState["FECURRENTINDEX"] = 1;
                ViewState["DCEPAGENUMBER"] = 1;
                ViewState["DCESORTEXPRESSION"] = null;
                ViewState["DCESORTDIRECTION"] = null;
                ViewState["DCECURRENTINDEX"] = 1;

                gvCrewLicence.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvFalgEndorsement.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvDCE.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void CrewLicence_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("NDS"))
            {
                Response.Redirect("CrewLicenceArchived.aspx?empid=" + strEmployeeId + "&type=1", false);
            }
            else if (CommandName.ToUpper().Equals("FES"))
            {
                Response.Redirect("CrewLicenceArchived.aspx?empid=" + strEmployeeId + "&type=2", false);
            }
            else if (CommandName.ToUpper().Equals("DCE"))
            {
                Response.Redirect("CrewLicenceArchived.aspx?empid=" + strEmployeeId + "&type=3", false);
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
            string[] alColumns = { "FLDLICENCENAME", "FLDLICENCENUMBER", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDPLACEOFISSUE" };
            string[] alCaptions = { "Licence", "Number", "Issue Date", "Expiry Date", "Place Of Issue" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewLicence.CrewLicenceSearch(
                        Int32.Parse(strEmployeeId), 0, 0
                        , sortexpression, sortdirection
                        , int.Parse(ViewState["PAGENUMBER"].ToString())
                        , gvCrewLicence.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount);
            General.SetPrintOptions("gvCrewLicence", "Crew Licence", alCaptions, alColumns, ds);

            gvCrewLicence.DataSource = ds;
            gvCrewLicence.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    private void BindFEData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDLICENCENAME", "FLDLICENCENUMBER", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDPLACEOFISSUE", "FLDFLAGNAME" };
            string[] alCaptions = { "Licence", "Licence Number", "Issue Date", "Expiry Date", "Place Of Issue", "Flag" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["FESORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["FESORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewLicenceFlagEndorsement.CrewLicenceFlagEndorsementSearch(General.GetNullableInteger(strEmployeeId),
                         null, 0
                        , sortexpression, sortdirection
                        , int.Parse(ViewState["FEPAGENUMBER"].ToString())
                        , gvFalgEndorsement.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount);

            gvFalgEndorsement.DataSource = ds;
            gvFalgEndorsement.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindDCEData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDLICENCENAME", "FLDLICENCENUMBER", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDPLACEOFISSUE" };
            string[] alCaptions = { "Licence", "Number", "Issue Date", "Expiry Date", "Place Of Issue" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewLicence.CrewLicenceSearch(
                        Int32.Parse(strEmployeeId), 1, 0
                        , sortexpression, sortdirection
                       , int.Parse(ViewState["DCEPAGENUMBER"].ToString())
                        , gvDCE.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount);

            gvDCE.DataSource = ds;
            gvDCE.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvCrewLicence_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewLicence.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewLicence_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            if (e.CommandName.ToString().ToUpper() == "DEARCHIVE")
            {
                string licenceid = ((RadLabel)e.Item.FindControl("lblLicenceId")).Text;
                PhoenixCrewLicence.ArchiveCrewLicence(int.Parse(licenceid), 1);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);
                BindData();
                gvCrewLicence.Rebind();              
            }
            if (e.CommandName.ToString().ToUpper() == "DELETE")
            {
                string licenceid = ((RadLabel)e.Item.FindControl("lblLicenceId")).Text;

                PhoenixCrewLicence.DeleteCrewLicence(
                   Convert.ToInt32(licenceid)
                    , int.Parse(Filter.CurrentCrewSelection));
                BindData();
                gvCrewLicence.Rebind();
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

    protected void gvCrewLicence_ItemDataBound(object sender, GridItemEventArgs e)
    {
        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null)
        {
            del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
            del.Attributes.Add("onclick", "return fnConfirmDelete(event)");
        }
        LinkButton cmdArchive = (LinkButton)e.Item.FindControl("cmdArchive");
        if (cmdArchive != null)
        {
            cmdArchive.Attributes.Add("onclick", "return fnConfirmDelete(event,'De-Archive selected document ?')");
            cmdArchive.Visible = SessionUtil.CanAccess(this.ViewState, cmdArchive.CommandName);
        }
        LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
        if (cmdEdit != null)
            cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

        LinkButton cmdAttachment = (LinkButton)e.Item.FindControl("cmdAtt");
        RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
        RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
        if (cmdAttachment != null)
        {
            cmdAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachment.CommandName);
            if (lblIsAtt.Text == string.Empty)
            {
                HtmlGenericControl html = new HtmlGenericControl();
                html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                cmdAttachment.Controls.Add(html);
            }
            cmdAttachment.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text.Trim() + "&mod=" + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.LICENCE + "&u=n'); return false;");
        }
        RadLabel expdate = e.Item.FindControl("lblExpiryDate") as RadLabel;
        System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
        if (expdate != null)
        {
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
        LinkButton imgRemarks = (LinkButton)e.Item.FindControl("imgRemarks");
        RadLabel lbl = (RadLabel)e.Item.FindControl("lblLicenceId");
        RadLabel lblR = (RadLabel)e.Item.FindControl("lblRemarks");
        if (imgRemarks != null)
        {
            if (lblR.Text == string.Empty)
            {
                HtmlGenericControl html = new HtmlGenericControl();
                html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-glasses-nr\"></i></span>";
                imgRemarks.Controls.Add(html);
            }
            imgRemarks.Attributes.Add("onclick", "javascript:openNewWindow('Codehelp1','','" + Session["sitepath"] + "/Crew/CrewMoreInfo.aspx?id=" + lbl.Text + "&type=" + PhoenixCrewAttachmentType.LICENCE + "&s=n','xlarge')");
        }

        RadLabel lbtn = (RadLabel)e.Item.FindControl("lblVerified");
        if (lbtn != null)
        {
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipLicense");
            lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
        }
    }

    protected void gvFalgEndorsement_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["FEPAGENUMBER"] = ViewState["FEPAGENUMBER"] != null ? ViewState["FEPAGENUMBER"] : gvFalgEndorsement.CurrentPageIndex + 1;
            BindFEData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFalgEndorsement_ItemDataBound(object sender, GridItemEventArgs e)
    {
        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null)
        {
            del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
            del.Attributes.Add("onclick", "return fnConfirmDelete(event)");
        }
        LinkButton cmdArchive = (LinkButton)e.Item.FindControl("cmdArchive");
        if (cmdArchive != null)
        {
            cmdArchive.Attributes.Add("onclick", "return fnConfirmDelete(event,'De-Archive selected document ?')");
            cmdArchive.Visible = SessionUtil.CanAccess(this.ViewState, cmdArchive.CommandName);
        }
        LinkButton cmdAttachment = (LinkButton)e.Item.FindControl("cmdAtt");
        RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
        RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
        if (cmdAttachment != null)
        {
            cmdAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachment.CommandName);
            if (lblIsAtt.Text == string.Empty)
            {
                HtmlGenericControl html = new HtmlGenericControl();
                html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                cmdAttachment.Controls.Add(html);
            }
            cmdAttachment.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text.Trim() + "&mod=" + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.LICENCE + "&u=n'); return false;");
        }
        RadLabel expdate = e.Item.FindControl("lblExpiryDate") as RadLabel;
        System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
        if (expdate != null)
        {
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

    protected void gvFalgEndorsement_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            if (e.CommandName.ToString().ToUpper() == "DEARCHIVE")
            {
                string licenceid = ((RadLabel)e.Item.FindControl("lblEndorsementId")).Text;
                PhoenixCrewLicenceFlagEndorsement.ArchiveCrewLicenceFlagEndorsement(int.Parse(licenceid), 1);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);
                BindFEData();
                gvFalgEndorsement.Rebind();
                //SetPageNavigator();
            }
            if (e.CommandName.ToString().ToUpper() == "DELETE")
            {
                string endorsementno = ((RadLabel)e.Item.FindControl("lblEndorsementId")).Text;

                PhoenixCrewLicenceFlagEndorsement.DeleteCrewLicenceFlagEndorsement(Convert.ToInt32(endorsementno), int.Parse(Filter.CurrentCrewSelection));
                BindFEData();
                gvFalgEndorsement.Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["FEPAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvDCE_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["DCEPAGENUMBER"] = ViewState["DCEPAGENUMBER"] != null ? ViewState["DCEPAGENUMBER"] : gvDCE.CurrentPageIndex + 1;
            BindDCEData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDCE_ItemDataBound(object sender, GridItemEventArgs e)
    {
        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null)
        {
            del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
            del.Attributes.Add("onclick", "return fnConfirmDelete(event)");
        }
        LinkButton cmdArchive = (LinkButton)e.Item.FindControl("cmdArchive");
        if (cmdArchive != null)
        {
            cmdArchive.Attributes.Add("onclick", "return fnConfirmDelete(event,'De-Archive selected document ?')");
            cmdArchive.Visible = SessionUtil.CanAccess(this.ViewState, cmdArchive.CommandName);
        }
        LinkButton cmdAttachment = (LinkButton)e.Item.FindControl("cmdAtt");
        RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
        RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
        if (cmdAttachment != null)
        {
            cmdAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachment.CommandName);
            if (lblIsAtt.Text == string.Empty)
            {
                HtmlGenericControl html = new HtmlGenericControl();
                html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                cmdAttachment.Controls.Add(html);
            }
            cmdAttachment.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text.Trim() + "&mod=" + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.LICENCE + "&u=n'); return false;");
        }
        RadLabel expdate = e.Item.FindControl("lblExpiryDate") as RadLabel;
        System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
        if (expdate != null)
        {
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

    protected void gvDCE_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            if (e.CommandName.ToString().ToUpper() == "DEARCHIVE")
            {
                string licenceid = ((RadLabel)e.Item.FindControl("lblLicenceId")).Text;
                PhoenixCrewLicence.ArchiveCrewLicence(int.Parse(licenceid), 1);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);
                BindDCEData();
                gvDCE.Rebind();
            }
            if (e.CommandName.ToString().ToUpper() == "DELETE")
            {
                string licenceid = ((RadLabel)e.Item.FindControl("lblLicenceId")).Text;

                PhoenixCrewLicence.DeleteCrewLicence(
                    Convert.ToInt32(licenceid)
                    , int.Parse(Filter.CurrentCrewSelection));
                BindDCEData();
                gvDCE.Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["DCEPAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
}
